using System.Text.RegularExpressions;

namespace QTip.Api.Services;

public class PatternMatchingService : IPatternMatchingService
{
    public List<string> GetMatches(string input, ClassificationTypes patternType)
    {
        Regex regex = GetRegexExpression(patternType);
        return regex.Matches(input).Select(m => m.Value).Distinct().ToList();   
    }

    private Regex GetRegexExpression(ClassificationTypes type)
    {
        switch (type)
        {
            case ClassificationTypes.Email:
                return new(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", RegexOptions.Compiled);;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), $"No regex defined for type {type}");
        }
    }
}