namespace Pointr.Api.Domain;

/// <summary>Site bilgisini ve siteye bağlı building listesini temsil eder.</summary>
public sealed record Site(
    Guid Id,
    string Name,
    string CampusCode,
    string Address);
