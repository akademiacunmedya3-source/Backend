using System.Net;
using System.Text.Json;
using FluentValidation;
using Harfistan.Application.Exceptions;

namespace Harfistan.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, $"An error occurred: {exception.Message}");

        var (statusCode, message, errors) = exception switch
        {
            NotFoundException notFound => (HttpStatusCode.NotFound, notFound.Message, null),
            AlreadyExistsException alreadyExistsException => (HttpStatusCode.Conflict, alreadyExistsException.Message,
                null),
            ValidationException validationException => (HttpStatusCode.BadRequest, "Validation failed",
                validationException.Errors.Select(e => e.ErrorMessage).ToList()),
            InvalidOperationException invalidOperationException => (HttpStatusCode.BadRequest,
                invalidOperationException.Message, null),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access", null),

            _ => (HttpStatusCode.InternalServerError, "An error occured while processing your request",
                null as List<string>)
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            errors,
            timestamp = DateTime.UtcNow
        };
        
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        await context.Response.WriteAsync(json);
    }
}