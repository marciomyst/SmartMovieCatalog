using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Domain.Tests.Users;

public sealed class UserRoleTests
{
    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.User)]
    public void Create_WithAllowedRole_ReturnsRole(string roleName)
    {
        UserRole role = UserRole.Create(roleName);

        Assert.Equal(roleName, role.Name);
        Assert.Equal(roleName, role.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("Manager")]
    [InlineData("admin")]
    public void Create_WithInvalidRole_Throws(string roleName)
    {
        Assert.Throws<ArgumentException>(() => UserRole.Create(roleName));
    }

    [Fact]
    public void Allowed_ExposesKnownRoles()
    {
        Assert.Contains(UserRole.Admin, UserRole.Allowed);
        Assert.Contains(UserRole.User, UserRole.Allowed);
    }
}
