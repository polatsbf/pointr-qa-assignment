namespace Pointr.Api.Contracts;

/// <summary>Level endpointlerinden dönen response modelidir.</summary>
public sealed record LevelResponse(
    Guid Id,
    Guid BuildingId,
    string Name,
    int Ordinal);
