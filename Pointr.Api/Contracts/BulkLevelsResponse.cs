namespace Pointr.Api.Contracts;

public sealed record BulkLevelsResponse(
    Guid BuildingId,
    int ImportedCount,
    IReadOnlyList<LevelResponse> Levels);
