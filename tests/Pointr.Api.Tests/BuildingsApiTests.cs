using System.Net;
using System.Net.Http.Json;
using Pointr.Api.Contracts;

namespace Pointr.Api.Tests;

public sealed class BuildingsApiTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateBuilding_AndGetBuilding_ReturnsExpectedResponses()
    {
        var site = await CreateSiteAsync();

        var createResponse = await _client.PostAsJsonAsync($"/api/sites/{site.Id}/buildings", new ImportBuildingRequest
        {
            Name = "Main Tower",
            ExternalReference = "BLD-001"
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdBuilding = await createResponse.Content.ReadFromJsonAsync<BuildingResponse>();
        Assert.NotNull(createdBuilding);

        var getResponse = await _client.GetAsync($"/api/sites/{site.Id}/buildings/{createdBuilding!.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var building = await getResponse.Content.ReadFromJsonAsync<BuildingResponse>();
        Assert.NotNull(building);
        Assert.Equal(site.Id, building!.SiteId);
        Assert.Equal("Main Tower", building.Name);
        Assert.Equal("BLD-001", building.ExternalReference);
    }

    [Fact]
    public async Task CreateBuilding_WhenSiteIsMissing_ReturnsNotFound()
    {
        var response = await _client.PostAsJsonAsync($"/api/sites/{Guid.NewGuid()}/buildings", new ImportBuildingRequest
        {
            Name = "Ghost Building",
            ExternalReference = "BLD-404"
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBuilding_RemovesExistingBuilding()
    {
        var site = await CreateSiteAsync();

        var createResponse = await _client.PostAsJsonAsync($"/api/sites/{site.Id}/buildings", new ImportBuildingRequest
        {
            Name = "West Block",
            ExternalReference = "BLD-WEST"
        });

        var building = await createResponse.Content.ReadFromJsonAsync<BuildingResponse>();
        Assert.NotNull(building);

        var deleteResponse = await _client.DeleteAsync($"/api/sites/{site.Id}/buildings/{building!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/sites/{site.Id}/buildings/{building.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateBuilding_WhenPayloadIsInvalid_ReturnsBadRequest()
    {
        var site = await CreateSiteAsync();

        var response = await _client.PostAsJsonAsync($"/api/sites/{site.Id}/buildings", new ImportBuildingRequest
        {
            Name = string.Empty,
            ExternalReference = "A"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<SiteResponse> CreateSiteAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/sites", new ImportSiteRequest
        {
            Name = "Campus Alpha",
            CampusCode = "CMP-01",
            Address = "10 Alpha Street"
        });

        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<SiteResponse>())!;
    }
}
