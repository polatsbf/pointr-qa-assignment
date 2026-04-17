using System.ComponentModel.DataAnnotations;

namespace Pointr.Api.Contracts;

public sealed class ImportSiteRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string CampusCode { get; init; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Address { get; init; } = string.Empty;
}
