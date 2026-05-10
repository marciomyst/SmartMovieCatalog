using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Tests.Common;

public sealed class GuardTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AgainstNullOrWhiteSpace_WithBlankValue_ThrowsArgumentException(string? value)
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => Guard.AgainstNullOrWhiteSpace(value, "title"));

        Assert.Equal("title", exception.ParamName);
        Assert.Contains("null or whitespace", exception.Message);
    }
}
