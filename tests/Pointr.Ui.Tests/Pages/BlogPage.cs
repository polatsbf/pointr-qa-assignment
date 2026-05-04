using Microsoft.Playwright;

namespace Pointr.Ui.Tests.Pages;

/// <summary>Blog sayfasındaki locator'ları ve page interaction methodlarını tutar.</summary>
public sealed class BlogPage(IPage page)
{
    private const string BlogUrl = "https://www.pointr.tech/blog";

    private ILocator LatestHeading => page.GetByRole(AriaRole.Heading, new() { Name = "Latest" });
    private ILocator ReadMoreLinks => page.GetByRole(AriaRole.Link, new() { Name = "Read more" });
    private ILocator ArticleTitle => page.Locator("main h1");
    private ILocator MainContent => page.Locator("main");

    public async Task OpenAsync()
    {
        await page.GotoAsync(BlogUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await LatestHeading.WaitForAsync();
    }

    public async Task<IReadOnlyList<string>> GetLatestArticleLinksAsync()
    {
        var allLinks = await GetAllArticleLinksAsync();

        return allLinks
            .Take(3)
            .ToArray();
    }

    public async Task<IReadOnlyList<string>> GetAllArticleLinksAsync()
    {
        var allLinks = await ReadMoreLinks.EvaluateAllAsync<string[]>(
            @"elements => elements
                .map(element => element.href)
                .filter(href => href.includes('/blog/'))");

        return allLinks
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(link => !string.Equals(link, BlogUrl, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    public async Task<string> GetArticleContentAsync(string articleLink)
    {
        await page.GotoAsync(articleLink, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await ArticleTitle.WaitForAsync();
        return await MainContent.InnerTextAsync();
    }
}
