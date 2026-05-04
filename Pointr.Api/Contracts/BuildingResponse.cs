namespace Pointr.Api.Contracts;

/// <summary>Building endpointlerinden dönen response modelidir.</summary>
public sealed record BuildingResponse(
    Guid Id,
    Guid SiteId,
    string Name,
    string ExternalReference);
