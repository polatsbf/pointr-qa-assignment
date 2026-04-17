namespace Pointr.Api.Contracts;

public sealed record SiteResponse(
    Guid Id,
    string Name,
    string CampusCode,
    string Address);
