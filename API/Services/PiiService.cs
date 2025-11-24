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
    private readonly IPatternMatchingService patternService;

    public PiiService(IPatternMatchingService patternService, AppDBContext dbContext)
    {
        this.dbContext = dbContext;
        this.patternService = patternService;
    }

    public async Task<ProcessingResult> ProcessPiiAsync(string rawInput)
    {
        // Get data matches
        List<string> matches = patternService.GetMatches(rawInput, ClassificationTypes.Email);

        // Set variables
        List<PiiVault> vaultList = new List<PiiVault>();
        string processedInput = rawInput;

        // Tokenize data
        processedInput = TokenizeData(matches, vaultList, processedInput);

        // Store submission
        TokenSubmission submission = new TokenSubmission
        {
            TokenizedContent = processedInput
        };

        dbContext.Submissions.Add(submission);
        await dbContext.SaveChangesAsync();

        // Store tokens in vault linked to submission id and return
        await AddTokensToVault(vaultList, submission);
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

    public async Task<PiiStats> GetPiiCountAsync(ClassificationTypes type)
    {
        var stats = new PiiStats
        {
            EmailCount = await dbContext.PiiVault.CountAsync(x => x.Classification == GetClassification(type))
        };
        return stats;
    }

    private static string TokenizeData(List<string> matches, List<PiiVault> vaultList, string processedInput)
    {
        foreach (string data in matches)
        {
            PiiVault vaultEntry = new PiiVault
            {
                Token = $"{{PII-{Guid.NewGuid().ToString().Substring(0, 16)}}}",
                OriginalValue = data,
                Classification = GetClassification(ClassificationTypes.Email)
            };

            vaultList.Add(vaultEntry);
            processedInput = processedInput.Replace(data, vaultEntry.Token);
        }

        return processedInput;
    }

    private async Task AddTokensToVault(List<PiiVault> vaultList, TokenSubmission submission)
    {
        foreach (var item in vaultList)
        {
            item.SubmissionId = submission.Id;
            dbContext.PiiVault.Add(item);
        }
        await dbContext.SaveChangesAsync();
    }

    private static string GetClassification(ClassificationTypes value)
    {
        switch (value)
        {
            case ClassificationTypes.Email:
                return "pii.email";
            default:
                throw new ArgumentException("Unsupported PII type");
        }
    }
}