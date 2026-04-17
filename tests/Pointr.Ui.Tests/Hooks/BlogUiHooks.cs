using Pointr.Ui.Tests.Support;
using Reqnroll;

namespace Pointr.Ui.Tests.Hooks;

[Binding]
public sealed class BlogUiHooks(BlogUiScenarioContext context, ScenarioContext scenarioContext)
{
    [AfterScenario]
    public async Task CleanupAsync()
    {
        var screenshotPath = await TakeFailureScreenshotAsync();

        ExtentReportManager.RecordScenarioResult(
            scenarioContext.ScenarioInfo.Title,
            context.BrowserName,
            scenarioContext.TestError,
            screenshotPath);

        if (context.Page is not null)
        {
            await context.Page.CloseAsync();
        }

        if (context.Browser is not null)
        {
            await context.Browser.CloseAsync();
        }

        context.Playwright?.Dispose();
    }

    [AfterTestRun]
    public static void FlushReport()
    {
        ExtentReportManager.Flush();
    }

    private async Task<string?> TakeFailureScreenshotAsync()
    {
        if (scenarioContext.TestError is null || context.Page is null)
        {
            return null;
        }

        Directory.CreateDirectory(ExtentReportManager.ScreenshotDirectory);

        var fileName = $"{SanitizeFileName(scenarioContext.ScenarioInfo.Title)}-{context.BrowserName}-{DateTime.UtcNow:yyyyMMddHHmmss}.png";
        var screenshotPath = Path.Combine(ExtentReportManager.ScreenshotDirectory, fileName);

        await context.Page.ScreenshotAsync(new()
        {
            Path = screenshotPath,
            FullPage = true
        });

        return screenshotPath;
    }

    private static string SanitizeFileName(string value)
    {
        foreach (var invalidCharacter in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(invalidCharacter, '-');
        }

        return value.Replace(' ', '-').ToLowerInvariant();
    }
}
