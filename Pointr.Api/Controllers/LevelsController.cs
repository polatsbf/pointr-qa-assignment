using Microsoft.AspNetCore.Mvc;
using Pointr.Api.Contracts;
using Pointr.Api.Domain;
using Pointr.Api.Infrastructure;

namespace Pointr.Api.Controllers;

/// <summary>Building altındaki tekli ve toplu level import endpointlerini yönetir.</summary>
[ApiController]
[Route("api/buildings/{buildingId:guid}/levels")]
public sealed class LevelsController(ISiteRepository repository) : ControllerBase
{
    [HttpPost]
    public ActionResult<LevelResponse> Import(Guid buildingId, [FromBody] ImportLevelRequest request)
    {
        var level = repository.AddLevel(new Level(
            Guid.NewGuid(),
            buildingId,
            request.Name,
            request.Ordinal));

        if (level is null)
        {
            return NotFound();
        }

        return Created($"/api/buildings/{buildingId}/levels/{level.Id}", ToResponse(level));
    }

    [HttpPost("bulk")]
    public ActionResult<BulkLevelsResponse> ImportBulk(Guid buildingId, [FromBody] ImportLevelsRequest request)
    {
        if (request.Levels.Select(x => x.Ordinal).Distinct().Count() != request.Levels.Count)
        {
            ModelState.AddModelError(nameof(request.Levels), "Level ordinals must be unique.");
            return ValidationProblem(ModelState);
        }

        var levels = request.Levels
            .Select(level => new Level(Guid.NewGuid(), buildingId, level.Name, level.Ordinal))
            .ToArray();

        var importedLevels = repository.AddLevels(buildingId, levels);
        if (importedLevels is null)
        {
            return NotFound();
        }

        var responseLevels = importedLevels.Select(ToResponse).ToArray();
        return Created($"/api/buildings/{buildingId}/levels", new BulkLevelsResponse(buildingId, responseLevels.Length, responseLevels));
    }

    private static LevelResponse ToResponse(Level level)
    {
        return new LevelResponse(level.Id, level.BuildingId, level.Name, level.Ordinal);
    }
}
