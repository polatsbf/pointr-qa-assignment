namespace Pointr.Api.Contracts;

public sealed record BuildingResponse(
    Guid Id,
    Guid SiteId,
    string Name,
    string ExternalReference);
