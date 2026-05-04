using System.Net;
using System.Net.Http.Json;
using Pointr.Api.Contracts;

namespace Pointr.Api.Tests;

/// <summary>Site API için pozitif ve negatif integration testlerini içerir.</summary>
public sealed class SitesApiTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateSite_AndGetSite_ReturnsExpectedResponses()
    {
        var createRequest = new ImportSiteRequest
        {
            Name = "Antalya Hospital Campus",
            CampusCode = "AHC-001",
            Address = "100 Antalya Health Street"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/sites", createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdSite = await createResponse.Content.ReadFromJsonAsync<SiteResponse>();
        Assert.NotNull(createdSite);

        var getResponse = await _client.GetAsync($"/api/sites/{createdSite!.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var site = await getResponse.Content.ReadFromJsonAsync<SiteResponse>();
        Assert.NotNull(site);
        Assert.Equal(createRequest.Name, site!.Name);
        Assert.Equal(createRequest.CampusCode, site.CampusCode);
        Assert.Equal(createRequest.Address, site.Address);
    }

    [Fact]
    public async Task GetSite_WhenMissing_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/sites/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteSite_RemovesExistingSite()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/sites", new ImportSiteRequest
        {
            Name = "East Campus",
            CampusCode = "EC-01",
            Address = "45 Sample Avenue"
        });

        var site = await createResponse.Content.ReadFromJsonAsync<SiteResponse>();
        Assert.NotNull(site);

        var deleteResponse = await _client.DeleteAsync($"/api/sites/{site!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/sites/{site.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateSite_WhenPayloadIsInvalid_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/sites", new ImportSiteRequest
        {
            Name = string.Empty,
            CampusCode = "A",
            Address = "123"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
