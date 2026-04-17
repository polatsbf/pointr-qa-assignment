using System.Text.RegularExpressions;

namespace Pointr.Ui.Tests.Support;

internal static partial class WordFrequencyAnalyzer
{
    private static readonly HashSet<string> StopWords =
    [
        "a", "an", "and", "are", "as", "at", "be", "been", "but", "by", "for",
        "from", "had", "has", "have", "if", "in", "into", "is", "it", "its",
        "of", "on", "or", "that", "the", "their", "there", "they", "this",
        "to", "was", "we", "were", "will", "with", "you", "your"
    ];

    public static IReadOnlyList<KeyValuePair<string, int>> GetTopWords(IEnumerable<string> texts, int take)
    {
        return texts
            .SelectMany(ExtractWords)
            .Where(word => word.Length >= 3)
            .Where(word => !StopWords.Contains(word))
            .GroupBy(word => word)
            .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))
            .OrderByDescending(pair => pair.Value)
            .ThenBy(pair => pair.Key, StringComparer.Ordinal)
            .Take(take)
            .ToArray();
    }

    public static string FormatForFile(IEnumerable<KeyValuePair<string, int>> topWords)
    {
        return string.Join(
            Environment.NewLine,
            topWords.Select(pair => $"{pair.Key}: {pair.Value}"));
    }

    private static IEnumerable<string> ExtractWords(string text)
    {
        foreach (Match match in WordRegex().Matches(text.ToLowerInvariant()))
        {
            yield return match.Value;
        }
    }

    [GeneratedRegex("[a-z]+", RegexOptions.Compiled)]
    private static partial Regex WordRegex();
}
