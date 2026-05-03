using System.ComponentModel.DataAnnotations;

namespace SmartMovieCatalog.Contracts.Auth;

public sealed record AuthenticateRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public string? Email { get; init; }

    [Required]
    [MinLength(1)]
    public string? Password { get; init; }
}
