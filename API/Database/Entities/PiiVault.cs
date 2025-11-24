namespace QTip.Api.Database.Entities;

public class PiiVault
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string OriginalValue { get; set; } = string.Empty;
    public string Classification { get; set; } = string.Empty;
    public int SubmissionId {get; set;}
}   