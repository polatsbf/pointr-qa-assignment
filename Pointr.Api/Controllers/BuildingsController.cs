using Microsoft.AspNetCore.Mvc;
using Pointr.Api.Contracts;
using Pointr.Api.Domain;
using Pointr.Api.Infrastructure;

namespace Pointr.Api.Controllers;

/// <summary>Site altındaki building oluşturma, getirme ve silme endpointlerini yönetir.</summary>
[ApiController]
[Route("api/sites/{siteId:guid}/buildings")]
public sealed class BuildingsController(ISiteRepository repository) : ControllerBase
{
    [HttpPost]
    public ActionResult<BuildingResponse> Import(Guid siteId, [FromBody] ImportBuildingRequest request)
    {
        var building = repository.AddBuilding(new Building(
            Guid.NewGuid(),
            siteId,
            request.Name,
            request.ExternalReference));

        if (building is null)
        {
            return NotFound();
        }

        var response = ToResponse(building);
        return CreatedAtAction(nameof(GetById), new { siteId, buildingId = response.Id }, response);
    }

    [HttpGet("{buildingId:guid}")]
    public ActionResult<BuildingResponse> GetById(Guid siteId, Guid buildingId)
    {
        var building = repository.GetBuilding(siteId, buildingId);
        if (building is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(building));
    }

    [HttpDelete("{buildingId:guid}")]
    public IActionResult Delete(Guid siteId, Guid buildingId)
    {
        return repository.DeleteBuilding(siteId, buildingId) ? NoContent() : NotFound();
    }

    private static BuildingResponse ToResponse(Building building)
    {
        return new BuildingResponse(building.Id, building.SiteId, building.Name, building.ExternalReference);
    }
}
