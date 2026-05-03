namespace SmartMovieCatalog.Api.Tests.TestSupport;

public static class TestJwtOptions
{
    public const string Issuer = "SmartMovieCatalog.Tests";
    public const string Audience = "SmartMovieCatalog.Api.Tests";
    public const string SigningKey = "0123456789abcdef0123456789abcdef";
    public const string WrongSigningKey = "abcdef0123456789abcdef0123456789";
}
