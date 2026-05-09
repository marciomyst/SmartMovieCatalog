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

    [Fact]
    public void From_WithDifferentGuid_CreatesDifferentIds()
    {
        UserId first = UserId.From(Guid.NewGuid());
        UserId second = UserId.From(Guid.NewGuid());

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Equals_WithNullOrDifferentType_ReturnsFalse()
    {
        UserId userId = UserId.New();

        Assert.False(userId.Equals(null));
        Assert.False(userId.Equals("not-a-user-id"));
    }
}
