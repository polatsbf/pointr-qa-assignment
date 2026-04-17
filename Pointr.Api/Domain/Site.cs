namespace Pointr.Api.Domain;

public sealed record Site(
    Guid Id,
    string Name,
    string CampusCode,
    string Address);
