namespace Pointr.Api.Domain;

public sealed record Level(
    Guid Id,
    Guid BuildingId,
    string Name,
    int Ordinal);
