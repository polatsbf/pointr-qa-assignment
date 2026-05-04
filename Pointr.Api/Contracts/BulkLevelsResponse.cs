namespace Pointr.Api.Contracts;

/// <summary>Toplu level import sonucunda dönen response modelidir.</summary>
public sealed record BulkLevelsResponse(
    Guid BuildingId,
    int ImportedCount,
    IReadOnlyList<LevelResponse> Levels);
