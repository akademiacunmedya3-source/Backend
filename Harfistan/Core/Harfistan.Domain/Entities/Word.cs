namespace Harfistan.Domain.Entities;

public sealed class Word
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Length { get; set; }
    public bool IsCommon { get; set; } = true;
    public string? Category { get; set; }
    public int DifficultyLevel { get; set; } = 1;
    public int TimesUsed { get; set; }
    public double AverageSuccessRate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUsedAt { get; set; }
    public List<DailyWord> DailyWords { get; set; } = new();
}