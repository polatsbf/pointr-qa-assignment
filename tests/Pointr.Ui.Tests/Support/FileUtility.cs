namespace Pointr.Ui.Tests.Support;

public static class FileUtility
{
    public static async Task WriteTopWordsReportAsync(string browserName, string content)
    {
        var outputDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Output"));
        Directory.CreateDirectory(outputDirectory);

        var browserSpecificFile = Path.Combine(
            outputDirectory,
            $"latest-3-articles-top-5-words-{browserName}.txt");

        await File.WriteAllTextAsync(browserSpecificFile, content);

        if (!string.Equals(browserName, "chromium", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var defaultFile = Path.Combine(outputDirectory, "latest-3-articles-top-5-words.txt");
        await File.WriteAllTextAsync(defaultFile, content);
    }
}
