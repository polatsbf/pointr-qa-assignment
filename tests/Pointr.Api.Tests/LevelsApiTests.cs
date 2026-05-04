using System.Net;
using System.Net.Http.Json;
using Pointr.Api.Contracts;

namespace Pointr.Api.Tests;

/// <summary>Level API için tekli ve toplu import testlerini içerir.</summary>
public sealed class LevelsApiTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ImportSingleLevel_ReturnsCreated()
    {
        var building = await CreateBuildingAsync();

        var response = await _client.PostAsJsonAsync($"/api/buildings/{building.Id}/levels", new ImportLevelRequest
        {
            Name = "Ground Floor",
            Ordinal = 0
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var level = await response.Content.ReadFromJsonAsync<LevelResponse>();
        Assert.NotNull(level);
        Assert.Equal(building.Id, level!.BuildingId);
        Assert.Equal("Ground Floor", level.Name);
        Assert.Equal(0, level.Ordinal);
    }

    [Fact]
    public async Task ImportMultipleLevels_ReturnsCreated()
    {
        var building = await CreateBuildingAsync();

        var response = await _client.PostAsJsonAsync($"/api/buildings/{building.Id}/levels/bulk", new ImportLevelsRequest
        {
            Levels =
            [
                new ImportLevelRequest { Name = "First Floor", Ordinal = 1 },
                new ImportLevelRequest { Name = "Second Floor", Ordinal = 2 }
            ]
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<BulkLevelsResponse>();
        Assert.NotNull(payload);
        Assert.Equal(building.Id, payload!.BuildingId);
        Assert.Equal(2, payload.ImportedCount);
        Assert.Equal(2, payload.Levels.Count);
    }

    [Fact]
    public async Task ImportMultipleLevels_WithDuplicateOrdinals_ReturnsBadRequest()
    {
        var building = await CreateBuildingAsync();

        var response = await _client.PostAsJsonAsync($"/api/buildings/{building.Id}/levels/bulk", new ImportLevelsRequest
        {
            Levels =
            [
                new ImportLevelRequest { Name = "Level A", Ordinal = 1 },
                new ImportLevelRequest { Name = "Level B", Ordinal = 1 }
            ]
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ImportLevel_WhenBuildingIsMissing_ReturnsNotFound()
    {
        var response = await _client.PostAsJsonAsync($"/api/buildings/{Guid.NewGuid()}/levels", new ImportLevelRequest
        {
            Name = "Ghost Level",
            Ordinal = 99
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ImportLevel_WhenPayloadIsInvalid_ReturnsBadRequest()
    {
        var building = await CreateBuildingAsync();

        var response = await _client.PostAsJsonAsync($"/api/buildings/{building.Id}/levels", new ImportLevelRequest
        {
            Name = string.Empty,
            Ordinal = 999
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<BuildingResponse> CreateBuildingAsync()
    {
        var siteResponse = await _client.PostAsJsonAsync("/api/sites", new ImportSiteRequest
        {
            Name = "Campus Beta",
            CampusCode = "CMP-02",
            Address = "20 Beta Street"
        });

        siteResponse.EnsureSuccessStatusCode();

        var site = (await siteResponse.Content.ReadFromJsonAsync<SiteResponse>())!;

        var buildingResponse = await _client.PostAsJsonAsync($"/api/sites/{site.Id}/buildings", new ImportBuildingRequest
        {
            Name = "Diagnostics Block",
            ExternalReference = "BLD-DIAG"
        });

        buildingResponse.EnsureSuccessStatusCode();
        return (await buildingResponse.Content.ReadFromJsonAsync<BuildingResponse>())!;
    }
}
