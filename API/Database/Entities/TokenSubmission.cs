namespace QTip.Api.Database.Entities;

public class TokenSubmission
{
    public int Id { get; set; }
    public string TokenizedContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}  