using System.ComponentModel.DataAnnotations;

namespace Pointr.Api.Contracts;

public sealed class ImportLevelsRequest
{
    [Required]
    [MinLength(1)]
    public IReadOnlyList<ImportLevelRequest> Levels { get; init; } = Array.Empty<ImportLevelRequest>();
}
