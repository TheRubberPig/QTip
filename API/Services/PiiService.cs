using Microsoft.EntityFrameworkCore;
using QTip.API.Database;
using QTip.Api.Database.DTOs;
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
        var submissionId = Guid.NewGuid();
        string processedInput = rawInput;
        foreach (var email in uniqueEmails)
        {
            var token = $"{{PII-{Guid.NewGuid().ToString().Substring(0,16)}}}";
            var vaultEntry = new Vault
            {
                Id = Guid.NewGuid(),
                Token = token,
                OriginalValue = email,
                Classification = "pii.email",
                SubmissionId = submissionId
            };

            dbContext.PiiVault.Add(vaultEntry);
            processedInput = processedInput.Replace(email, token);
        }

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            TokenizedContent = processedInput,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Submissions.Add(submission);
        return new ProcessingResult(processedInput);
    }

    public async Task<int> GetPiiCountAsync()
    {
        return await dbContext.PiiVault.CountAsync(x => x.Classification == "pii.email");
    }
}