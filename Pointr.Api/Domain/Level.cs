namespace Pointr.Api.Domain;

/// <summary>Bir building altındaki kat/level bilgisini temsil eder.</summary>
public sealed record Level(
    Guid Id,
    Guid BuildingId,
    string Name,
    int Ordinal);
