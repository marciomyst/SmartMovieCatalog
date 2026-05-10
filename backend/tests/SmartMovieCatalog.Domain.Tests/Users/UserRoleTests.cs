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
    [InlineData("Manager")]
    [InlineData("admin")]
    public void Create_WithInvalidRole_Throws(string roleName)
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => UserRole.Create(roleName));

        Assert.Equal("name", exception.ParamName);
        Assert.Contains("User role is not recognized.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankRole_Throws(string roleName)
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => UserRole.Create(roleName));

        Assert.Equal("name", exception.ParamName);
        Assert.Contains("null or whitespace", exception.Message);
    }

    [Fact]
    public void Allowed_ExposesKnownRoles()
    {
        Assert.Contains(UserRole.Admin, UserRole.Allowed);
        Assert.Contains(UserRole.User, UserRole.Allowed);
    }

    [Fact]
    public void Equals_IsBasedOnRoleName()
    {
        UserRole admin = UserRole.Create(UserRole.Admin);
        UserRole user = UserRole.Create(UserRole.User);

        Assert.NotEqual(admin, user);
    }
}
