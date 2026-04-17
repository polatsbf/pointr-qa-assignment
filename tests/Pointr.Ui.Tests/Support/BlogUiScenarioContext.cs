using Microsoft.Playwright;
using Pointr.Ui.Tests.Pages;

namespace Pointr.Ui.Tests.Support;

public sealed class BlogUiScenarioContext
{
    public string BrowserName { get; set; } = string.Empty;
    public IPlaywright? Playwright { get; set; }
    public IBrowser? Browser { get; set; }
    public IPage? Page { get; set; }
    public BlogPage? BlogPage { get; set; }
    public IReadOnlyList<string> AllArticleLinks { get; set; } = Array.Empty<string>();
    public IReadOnlyList<string> LatestArticleLinks { get; set; } = Array.Empty<string>();
    public IReadOnlyList<string> ArticleTexts { get; set; } = Array.Empty<string>();
    public IReadOnlyList<KeyValuePair<string, int>> TopWords { get; set; } = Array.Empty<KeyValuePair<string, int>>();
}
