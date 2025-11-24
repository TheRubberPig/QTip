namespace QTip.Api.Database.Entities;

public class TokenSubmission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TokenizedContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}  