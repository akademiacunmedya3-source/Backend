namespace Harfistan.Application.Commons;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string>? Errors { get; set; }

    private Result(bool isSuccess, T? data, string? errorMessage, List<string>? errors)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        Errors = errors;
    }

    public static Result<T> Success(T data) => new(true, data, null, null);
    public static Result<T> Failure(string errorMessage) => new(true, default, errorMessage, null);
    public static Result<T> Failure(List<string> errors) => new(true, default, null, errors);
}
public class Result
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string>? Errors { get; set; }
    
    private Result(bool isSuccess, string? errorMessage, List<string>? errors)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Errors = errors;
    }
    public static Result Success() => new(true, null, null);
    public static Result Failure(string errorMessage) => new(true, errorMessage, null);
    public static Result Failure(List<string> errors) => new(true, null, errors);
}