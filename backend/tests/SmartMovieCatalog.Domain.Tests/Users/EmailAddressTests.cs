using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Domain.Tests.Users;

public sealed class EmailAddressTests
{
    [Fact]
    public void Create_WithValidEmail_TrimsAndNormalizes()
    {
        EmailAddress email = EmailAddress.Create(" User@Example.com ");

        Assert.Equal("User@Example.com", email.Value);
        Assert.Equal("USER@EXAMPLE.COM", email.NormalizedValue);
        Assert.Equal("User@Example.com", email.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-an-email")]
    public void TryCreate_WithInvalidEmail_ReturnsFalse(string? value)
    {
        bool created = EmailAddress.TryCreate(value, out EmailAddress? emailAddress);

        Assert.False(created);
        Assert.Null(emailAddress);
    }

    [Fact]
    public void Create_WithInvalidEmail_Throws()
    {
        Assert.Throws<ArgumentException>(() => EmailAddress.Create("not-an-email"));
    }

    [Fact]
    public void Equals_UsesNormalizedValue()
    {
        EmailAddress first = EmailAddress.Create("user@example.com");
        EmailAddress second = EmailAddress.Create(" USER@EXAMPLE.COM ");

        Assert.Equal(first, second);
    }
}
