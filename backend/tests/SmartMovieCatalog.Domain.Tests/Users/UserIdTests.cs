using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Domain.Tests.Users;

public sealed class UserIdTests
{
    [Fact]
    public void New_ReturnsNonEmptyId()
    {
        UserId userId = UserId.New();

        Assert.NotEqual(Guid.Empty, userId.Value);
        Assert.Equal(userId.Value.ToString(), userId.ToString());
    }

    [Fact]
    public void From_WithEmptyGuid_Throws()
    {
        Assert.Throws<ArgumentException>(() => UserId.From(Guid.Empty));
    }

    [Fact]
    public void From_WithSameGuid_CreatesEqualIds()
    {
        Guid value = Guid.NewGuid();

        Assert.Equal(UserId.From(value), UserId.From(value));
    }
}
