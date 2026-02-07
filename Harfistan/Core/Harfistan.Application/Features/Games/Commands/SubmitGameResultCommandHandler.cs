using System.Text.Json;
using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.DTOs.Game;
using Harfistan.Application.Exceptions;
using Harfistan.Domain.Entities;
using Mediator;

namespace Harfistan.Application.Features.Games.Commands;

public record SubmitGameResultCommand : IRequest<GameResultDTO>
{
    public Guid UserId { get; set; }
    public bool IsWin { get; set; }
    public int Attempts { get; set; }
    public int DurationSeconds { get; set; }
    public List<string> Guesses { get; set; } = new();
    public bool IsHardMode { get; set; }
    public string? DeviceType { get; set; }
}

public class SubmitGameResultCommandHandler(IDailyWordRepository dailyWordRepository, IGameResultRepository gameResultRepository, IUserRepository userRepository): IRequestHandler<SubmitGameResultCommand, GameResultDTO>
{
    public async ValueTask<GameResultDTO> Handle(SubmitGameResultCommand request, CancellationToken cancellationToken)
    {
        var dailyWord = await dailyWordRepository.GetTodayAsync(cancellationToken) ?? throw new NotFoundException("Daily word not found for today");

        var existingResult =
            await gameResultRepository.GetByUserAndDailyWordAsync(request.UserId, dailyWord.Id, cancellationToken);

        if (existingResult is not null)
            throw new AlreadyExistsException($"User {request.UserId} has already played today");
        
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken) ?? throw new NotFoundException($"User", request.UserId);
        if (user.Stats is null)
            throw new NotFoundException($"User stats not found for user {request.UserId}");

        var gameResult = new GameResult
        {
            UserId = request.UserId,
            DailyWordId = dailyWord.Id,
            IsWin = request.IsWin,
            Attempts = request.Attempts,
            DurationSeconds = request.DurationSeconds,
            GuessesJson = JsonSerializer.Serialize(request.Guesses),
            IsHardMode = request.IsHardMode,
            DeviceType = request.DeviceType,
            PlayedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow
        };
        
        await gameResultRepository.AddAsync(gameResult, cancellationToken);
        
        dailyWord.TotalAttempts++;
        dailyWord.TotalPlayers++;
        
        if(request.IsWin) 
            dailyWord.TotalWins++;
        else
            dailyWord.TotalLosses++;
        
        dailyWord.WinRate = dailyWord.TotalAttempts > 0 
            ? Math.Round((double)dailyWord.TotalWins / dailyWord.TotalAttempts * 100, 2) : 0;

        if (request.IsWin)
        {
            var totalWinAttempts = (dailyWord.AverageAttempts * (dailyWord.TotalWins - 1)) + request.Attempts;
            dailyWord.AverageAttempts = Math.Round(totalWinAttempts / dailyWord.TotalWins, 2);
        }
        
        await dailyWordRepository.UpdateAsync(dailyWord, cancellationToken);
        
        await UpdateUserStat(user.Stats, request, cancellationToken);
        
        await dailyWordRepository.SaveChangesAsync(cancellationToken);

        return new GameResultDTO()
        {
            Success = true,
            Message = request.IsWin ? "Congratulations!" : "Better luck tomorrow!",
            GameResultId = gameResult.Id,
            UpdatedStats = MapToStatsDto(user.Stats),
            CorrectWord = request.IsWin ? null : dailyWord.Word.Text
        };
    }
    
    private async Task UpdateUserStat(UserStat stat, SubmitGameResultCommand request,
        CancellationToken cancellationToken)
    {
        stat.GamesPlayed++;

        if (request.IsWin)
            stat.GamesWon++;
        else
            stat.GamesLost++;

        stat.WinRate = stat.GamesPlayed > 0
            ? Math.Round((double)stat.GamesWon / stat.GamesPlayed * 100, 2) : 0;
        
        var today = DateTime.UtcNow.Date;
        var yesterday = today.AddDays(-1);

        if (request.IsWin)
        {
            if(stat.LastPlayedDate == yesterday)
                stat.CurrentStreak++;
            else if (stat.LastPlayedDate != today)
                stat.CurrentStreak = 1;


            if (stat.CurrentStreak > stat.MaxStreak)
                stat.MaxStreak = stat.CurrentStreak;
            
            stat.MaxStreak = Math.Max(stat.MaxStreak, stat.CurrentStreak);
        }
        else
            stat.CurrentStreak = 0;
        
        stat.LastPlayedDate = today;

        if (request.IsWin && request.Attempts >= 1 && request.Attempts <= 6)
        {
            var distribution = JsonSerializer.Deserialize<List<int>>(stat.GuessDistribution) ?? new List<int> { 0, 0, 0, 0, 0, 0 };
            distribution[request.Attempts - 1]++;
            stat.GuessDistribution = JsonSerializer.Serialize(distribution);
        }

        if (request.IsWin)
        {
            var totalWinAttempts = (stat.AverageAttempts * (stat.GamesWon - 1)) + request.Attempts;
            stat.AverageAttempts = Math.Round(totalWinAttempts / stat.GamesWon, 2);
        }
        
        stat.TotalPlayTimeSeconds += request.DurationSeconds;
        stat.UpdatedAt = DateTime.UtcNow;
        
    }
    
    private static UserStatsDto MapToStatsDto(UserStat stat)
    {
        var distribution = JsonSerializer.Deserialize<List<int>>(stat.GuessDistribution) ??
                           new List<int> { 0, 0, 0, 0, 0, 0 };

        return new UserStatsDto()
        {
            GamesPlayed = stat.GamesPlayed,
            GamesWon = stat.GamesWon,
            GamesLost = stat.GamesLost,
            WinRate = stat.WinRate,
            CurrentStreak = stat.CurrentStreak,
            MaxStreak = stat.MaxStreak,
            GuessDistribution = distribution,
            AverageAttempts = stat.AverageAttempts,
        };
    }

   
}