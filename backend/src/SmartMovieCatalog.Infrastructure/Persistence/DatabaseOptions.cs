using System.ComponentModel.DataAnnotations;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class DatabaseOptions
{
    public const string SectionName = "ConnectionStrings";

    [Required]
    public string? DefaultConnection { get; init; }
}
