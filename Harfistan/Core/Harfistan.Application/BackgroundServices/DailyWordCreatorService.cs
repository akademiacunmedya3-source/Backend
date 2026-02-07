using System.Runtime.InteropServices.JavaScript;
using Harfistan.Application.Features.DailyWords.Commands.CreateDailyWord;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Harfistan.Application.BackgroundServices;

public class DailyWordCreatorService(IServiceProvider serviceProvider, ILogger<DailyWordCreatorService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Daily Word Creator Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CreateTomorrowsDailyWord(stoppingToken);
            }catch (Exception e)
            {
                logger.LogError(e, "Error creating tomorrows daily word");
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task CreateTomorrowsDailyWord(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        try
        {
            var command = new CreateDailyWordCommand(tomorrow, null, 5);
            
            var dailyWordId = await mediator.Send(command, cancellationToken);
            logger.LogInformation($"Created daily word for {tomorrow} with ID {dailyWordId}");
        }
        catch (Exception e)
        {
            logger.LogDebug($"Daily word for {tomorrow} already exists:{e.Message}");
        }
    }
}