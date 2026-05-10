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

    [Fact]
    public void TryCreate_WithValidEmail_ReturnsTrueAndCreatesEmailAddress()
    {
        bool created = EmailAddress.TryCreate(" User@Example.com ", out EmailAddress? emailAddress);

        Assert.True(created);
        Assert.NotNull(emailAddress);
        Assert.Equal("User@Example.com", emailAddress!.Value);
    }

    [Fact]
    public void TryCreate_WithBoundaryLengthEmail_ReturnsTrue()
    {
        string localPart = new('a', 308);
        string value = $"{localPart}@example.com";

        bool created = EmailAddress.TryCreate(value, out EmailAddress? emailAddress);

        Assert.True(created);
        Assert.NotNull(emailAddress);
        Assert.Equal(value, emailAddress!.Value);
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
        ArgumentException exception = Assert.Throws<ArgumentException>(() => EmailAddress.Create("not-an-email"));

        Assert.Equal("value", exception.ParamName);
        Assert.Contains("Email address is invalid.", exception.Message);
    }

    [Fact]
    public void Equals_UsesNormalizedValue()
    {
        EmailAddress first = EmailAddress.Create("user@example.com");
        EmailAddress second = EmailAddress.Create(" USER@EXAMPLE.COM ");

        Assert.Equal(first, second);
    }

    [Fact]
    public void Equals_WithDifferentEmails_ReturnsFalse()
    {
        EmailAddress first = EmailAddress.Create("user@example.com");
        EmailAddress second = EmailAddress.Create("admin@example.com");

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Create_WithBoundaryLengthEmail_ReturnsEmailAddress()
    {
        string localPart = new('a', 308);
        string value = $"{localPart}@example.com";

        EmailAddress email = EmailAddress.Create(value);

        Assert.Equal(value, email.Value);
        Assert.Equal(value.ToUpperInvariant(), email.NormalizedValue);
    }

    [Fact]
    public void Create_WithTooLongEmail_Throws()
    {
        string localPart = new('a', 309);
        string value = $"{localPart}@example.com";

        ArgumentException exception = Assert.Throws<ArgumentException>(() => EmailAddress.Create(value));

        Assert.Equal("value", exception.ParamName);
        Assert.Contains("Email address is invalid.", exception.Message);
    }

    [Fact]
    public void TryCreate_WithTooLongEmail_ReturnsFalse()
    {
        string localPart = new('a', 309);
        string value = $"{localPart}@example.com";

        bool created = EmailAddress.TryCreate(value, out EmailAddress? emailAddress);

        Assert.False(created);
        Assert.Null(emailAddress);
    }
}
