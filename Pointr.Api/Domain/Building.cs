namespace Pointr.Api.Domain;

public sealed record Building(
    Guid Id,
    Guid SiteId,
    string Name,
    string ExternalReference);
