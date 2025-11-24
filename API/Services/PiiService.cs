using Microsoft.EntityFrameworkCore;
using QTip.API.Database;
using QTip.Api.Models;
using QTip.Api.Interfaces;
using QTip.Api.Database.Entities;
using System.Text.RegularExpressions;

namespace QTip.Api.Services;
public class PiiService : IPiiService
{
    private readonly AppDBContext dbContext;
    private Regex emailRegex = new(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", RegexOptions.Compiled);

    public PiiService(AppDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ProcessingResult> ProcessPiiAsync(string rawInput)
    {
        var matches = emailRegex.Matches(rawInput);
        var uniqueEmails = matches.Select(m => m.Value).Distinct().ToList();
        string processedInput = rawInput;
        foreach (var email in uniqueEmails)
        {
            var token = $"{{PII-{Guid.NewGuid().ToString().Substring(0,16)}}}";
            var vaultEntry = new PiiVault
            {
                Id = Guid.NewGuid(),
                Token = token,
                OriginalValue = email,
                Classification = "pii.email"
            };

            dbContext.PiiVault.Add(vaultEntry);
            processedInput = processedInput.Replace(email, token);
        }

        var submission = new TokenSubmission
        {
            Id = Guid.NewGuid(),
            TokenizedContent = processedInput,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Submissions.Add(submission);
        await dbContext.SaveChangesAsync();
        return new ProcessingResult(processedInput);
    }

    public async Task<PiiStats> GetPiiCountAsync()
    {
        var stats = new PiiStats
        {
            Total = await dbContext.PiiVault.CountAsync()
        };
        return stats;
    }

    public async Task<PiiStats> GetPiiCountAsync(string type)
    {
        var stats = new PiiStats
        {
            EmailCount = await dbContext.PiiVault.CountAsync(x => x.Classification == GetClassification(type))
        };
        return stats;
    }

    private string GetClassification(string value)
    {
        switch (value)
        {
            case "email":
                return "pii.email";
            default:
                throw new ArgumentException("Unsupported PII type");
        }
    }
}