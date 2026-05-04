namespace Pointr.Api.Domain;

/// <summary>Building bilgisini, bağlı olduğu siteyi ve level listesini temsil eder.</summary>
public sealed record Building(
    Guid Id,
    Guid SiteId,
    string Name,
    string ExternalReference);
