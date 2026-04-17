using Microsoft.Playwright;
using Pointr.Ui.Tests.Pages;
using Pointr.Ui.Tests.Support;
using Reqnroll;
using Xunit;

namespace Pointr.Ui.Tests.Steps;

[Binding]
public sealed class BlogAnalysisSteps(BlogUiScenarioContext context)
{
    [Given(@"I open the Pointr blog in ""(.*)""")]
    public async Task GivenIOpenThePointrBlogInBrowser(string browserName)
    {
        context.BrowserName = browserName;
        context.Playwright = await Playwright.CreateAsync();
        context.Browser = await BrowserUtility.LaunchAsync(context.Playwright, browserName);
        context.Page = await context.Browser.NewPageAsync();
        context.BlogPage = new BlogPage(context.Page);

        await context.BlogPage.OpenAsync();
    }

    [When(@"I collect the latest 3 article contents")]
    public async Task WhenICollectTheLatest3ArticleContents()
    {
        Assert.NotNull(context.BlogPage);

        context.LatestArticleLinks = await context.BlogPage!.GetLatestArticleLinksAsync();

        Assert.True(context.LatestArticleLinks.Count >= 3, "Expected at least 3 latest articles to be available.");

        var articleTexts = new List<string>();

        foreach (var articleLink in context.LatestArticleLinks.Take(3))
        {
            var mainContent = await context.BlogPage.GetArticleContentAsync(articleLink);
            Assert.False(string.IsNullOrWhiteSpace(mainContent));

            articleTexts.Add(mainContent);
        }

        context.ArticleTexts = articleTexts;
    }

    [When(@"I collect all article links")]
    public async Task WhenICollectAllArticleLinks()
    {
        Assert.NotNull(context.BlogPage);

        context.AllArticleLinks = await context.BlogPage!.GetAllArticleLinksAsync();
    }

    [When(@"I calculate the most repeated 5 words")]
    public void WhenICalculateTheMostRepeated5Words()
    {
        context.TopWords = WordFrequencyAnalyzer.GetTopWords(context.ArticleTexts, 5);
    }

    [Then(@"all articles should be loaded successfully")]
    public void ThenAllArticlesShouldBeLoadedSuccessfully()
    {
        Assert.NotEmpty(context.AllArticleLinks);
        Assert.True(context.AllArticleLinks.Count >= 3, "Expected at least 3 blog article links to be loaded.");
    }

    [Then(@"the latest 3 articles should be loaded successfully")]
    public void ThenTheLatest3ArticlesShouldBeLoadedSuccessfully()
    {
        Assert.Equal(3, context.ArticleTexts.Count);
        Assert.All(context.ArticleTexts, text => Assert.False(string.IsNullOrWhiteSpace(text)));
    }

    [Then(@"the top 5 words should be saved into a text file for ""(.*)""")]
    public async Task ThenTheTop5WordsShouldBeSavedIntoATextFileFor(string browserName)
    {
        Assert.Equal(5, context.TopWords.Count);
        Assert.All(context.TopWords, pair => Assert.True(pair.Value > 0));

        var content = WordFrequencyAnalyzer.FormatForFile(context.TopWords);
        await FileUtility.WriteTopWordsReportAsync(browserName, content);
    }
}
