using System.ComponentModel.DataAnnotations;

namespace Pointr.Api.Contracts;

/// <summary>Toplu level import isteğinde gelen request body modelidir.</summary>
public sealed class ImportLevelsRequest
{
    [Required]
    [MinLength(1)]
    public IReadOnlyList<ImportLevelRequest> Levels { get; init; } = Array.Empty<ImportLevelRequest>();
}
