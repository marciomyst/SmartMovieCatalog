using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Tests.Common;

public sealed class ResultTests
{
    [Fact]
    public void Success_CreatesSuccessfulResultWithoutError()
    {
        Result result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_CreatesFailedResultWithError()
    {
        Result result = Result.Failure("Invalid state.");

        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid state.", result.Error);
    }
}
