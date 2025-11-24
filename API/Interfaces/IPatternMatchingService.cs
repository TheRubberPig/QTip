public interface IPatternMatchingService
{
    List<string> GetMatches(string input, ClassificationTypes patternType);
}