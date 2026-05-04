using System.ComponentModel.DataAnnotations;

namespace Pointr.Api.Contracts;

/// <summary>Yeni building import isteğinde gelen request body modelidir.</summary>
public sealed class ImportBuildingRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string ExternalReference { get; init; } = string.Empty;
}
