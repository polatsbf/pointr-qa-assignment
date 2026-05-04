using System.ComponentModel.DataAnnotations;

namespace Pointr.Api.Contracts;

/// <summary>Tek level import isteğinde gelen request body modelidir.</summary>
public sealed class ImportLevelRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [Range(-20, 200)]
    public int Ordinal { get; init; }
}
