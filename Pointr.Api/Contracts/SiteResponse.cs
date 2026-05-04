namespace Pointr.Api.Contracts;

/// <summary>Site endpointlerinden dönen response modelidir.</summary>
public sealed record SiteResponse(
    Guid Id,
    string Name,
    string CampusCode,
    string Address);
