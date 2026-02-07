namespace Harfistan.Application.DTOs.Words;

public class WordValidationDTO
{
    public bool IsValid { get; set; }
    public string Word { get; set; } = string.Empty;
    public string? Message { get; set; }
}