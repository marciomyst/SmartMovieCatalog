using SmartMovieCatalog.Application;

namespace SmartMovieCatalog.Application.Tests;

public sealed class ResultTests
{
    [Fact]
    public void Success_CreatesSuccessfulResult()
    {
        Result<string, string> result = Result<string, string>.Success("value");

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal("value", result.Value);
        Assert.Equal("success:value", result.Match(
            value => $"success:{value}",
            error => $"failure:{error}"));
    }

    [Fact]
    public void Failure_CreatesFailedResult()
    {
        Result<string, string> result = Result<string, string>.Failure("error");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("error", result.Error);
        Assert.Equal("failure:error", result.Match(
            value => $"success:{value}",
            error => $"failure:{error}"));
    }

    [Fact]
    public void Value_OnFailedResult_Throws()
    {
        Result<string, string> result = Result<string, string>.Failure("error");

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Error_OnSuccessfulResult_Throws()
    {
        Result<string, string> result = Result<string, string>.Success("value");

        Assert.Throws<InvalidOperationException>(() => result.Error);
    }
}
