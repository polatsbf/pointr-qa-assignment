namespace Pointr.Api.Contracts;

public sealed record LevelResponse(
    Guid Id,
    Guid BuildingId,
    string Name,
    int Ordinal);
