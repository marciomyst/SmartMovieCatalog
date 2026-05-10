using SmartMovieCatalog.Application.Abstractions.Persistence;

namespace SmartMovieCatalog.Application.Tests.Abstractions.Persistence;

public sealed class PagedResultTests
{
    [Fact]
    public void TotalPages_WithNoItems_ReturnsZero()
    {
        PagedResult<string> result = new([], 1, 12, 0);

        Assert.Equal(0, result.TotalPages);
        Assert.False(result.HasPreviousPage);
        Assert.False(result.HasNextPage);
    }

    [Fact]
    public void PaginationFlags_OnFirstPage_ReportsNextPageOnly()
    {
        PagedResult<string> result = new(["a"], 1, 1, 2);

        Assert.Equal(2, result.TotalPages);
        Assert.False(result.HasPreviousPage);
        Assert.True(result.HasNextPage);
    }

    [Fact]
    public void PaginationFlags_OnLastPage_ReportsPreviousPageOnly()
    {
        PagedResult<string> result = new(["b"], 2, 1, 2);

        Assert.Equal(2, result.TotalPages);
        Assert.True(result.HasPreviousPage);
        Assert.False(result.HasNextPage);
    }
}
