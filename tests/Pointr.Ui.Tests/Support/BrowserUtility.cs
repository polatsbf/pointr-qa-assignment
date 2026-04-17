using Microsoft.Playwright;

namespace Pointr.Ui.Tests.Support;

public static class BrowserUtility
{
    public static async Task<IBrowser> LaunchAsync(IPlaywright playwright, string browserName)
    {
        var options = new BrowserTypeLaunchOptions
        {
            Headless = !IsHeadedModeEnabled()
        };

        return browserName switch
        {
            "chromium" => await playwright.Chromium.LaunchAsync(options),
            "firefox" => await playwright.Firefox.LaunchAsync(options),
            _ => throw new ArgumentOutOfRangeException(nameof(browserName), browserName, "Unsupported browser.")
        };
    }

    private static bool IsHeadedModeEnabled()
    {
        return string.Equals(
            Environment.GetEnvironmentVariable("HEADED"),
            "true",
            StringComparison.OrdinalIgnoreCase);
    }
}
