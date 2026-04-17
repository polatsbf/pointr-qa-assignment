namespace Pointr.Ui.Tests.Support;

public static class FileUtility
{
    public static async Task WriteTopWordsReportAsync(string browserName, string content)
    {
        var artifactsDirectory = Path.Combine(AppContext.BaseDirectory, "artifacts");
        Directory.CreateDirectory(artifactsDirectory);

        var browserSpecificFile = Path.Combine(
            artifactsDirectory,
            $"latest-3-articles-top-5-words-{browserName}.txt");

        await File.WriteAllTextAsync(browserSpecificFile, content);

        if (!string.Equals(browserName, "chromium", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var defaultFile = Path.Combine(artifactsDirectory, "latest-3-articles-top-5-words.txt");
        await File.WriteAllTextAsync(defaultFile, content);
    }
}
