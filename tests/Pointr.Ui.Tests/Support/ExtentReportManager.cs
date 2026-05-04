using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace Pointr.Ui.Tests.Support;

/// <summary>UI testleri için Extent HTML report oluşturur ve senaryo sonuçlarını rapora yazar.</summary>
public static class ExtentReportManager
{
    private static readonly object SyncRoot = new();
    private static readonly Lazy<ExtentReports> Report = new(CreateReport);

    public static string ReportDirectory { get; } = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Report"));
    public static string ScreenshotDirectory { get; } = Path.Combine(ReportDirectory, "screenshots");

    public static void RecordScenarioResult(string scenarioTitle, string browserName, Exception? error, string? screenshotPath)
    {
        lock (SyncRoot)
        {
            var testName = string.IsNullOrWhiteSpace(browserName)
                ? scenarioTitle
                : $"{scenarioTitle} [{browserName}]";

            var test = Report.Value.CreateTest(testName);

            if (!string.IsNullOrWhiteSpace(browserName))
            {
                test.AssignCategory(browserName);
            }

            if (error is null)
            {
                test.Pass("Scenario passed.");
                return;
            }

            test.Fail(error.Message);

            if (!string.IsNullOrWhiteSpace(screenshotPath))
            {
                test.AddScreenCaptureFromPath(screenshotPath);
            }
        }
    }

    public static void Flush()
    {
        lock (SyncRoot)
        {
            if (!Report.IsValueCreated)
            {
                return;
            }

            Report.Value.Flush();
        }
    }

    private static ExtentReports CreateReport()
    {
        Directory.CreateDirectory(ReportDirectory);
        Directory.CreateDirectory(ScreenshotDirectory);

        var reporter = new ExtentSparkReporter(Path.Combine(ReportDirectory, "report.html"));
        var report = new ExtentReports();
        report.AttachReporter(reporter);

        return report;
    }
}
