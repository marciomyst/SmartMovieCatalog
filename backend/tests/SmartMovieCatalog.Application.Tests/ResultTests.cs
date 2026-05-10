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
    public void Success_WithNullValue_ThrowsArgumentNullException()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => Result<string, string>.Success(null!));

        Assert.Equal("value", exception.ParamName);
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
    public void Failure_WithNullError_ThrowsArgumentNullException()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => Result<string, string>.Failure(null!));

        Assert.Equal("error", exception.ParamName);
    }

    [Fact]
    public void Value_OnFailedResult_Throws()
    {
        Result<string, string> result = Result<string, string>.Failure("error");

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => result.Value);

        Assert.Equal("Cannot access the value of a failed result.", exception.Message);
    }

    [Fact]
    public void Error_OnSuccessfulResult_Throws()
    {
        Result<string, string> result = Result<string, string>.Success("value");

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => result.Error);

        Assert.Equal("Cannot access the error of a successful result.", exception.Message);
    }

    [Fact]
    public void Match_WithNullSuccessDelegate_ThrowsArgumentNullException()
    {
        Result<string, string> result = Result<string, string>.Success("value");

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => result.Match(null!, error => $"failure:{error}"));

        Assert.Equal("onSuccess", exception.ParamName);
    }

    [Fact]
    public void Match_WithNullFailureDelegate_ThrowsArgumentNullException()
    {
        Result<string, string> result = Result<string, string>.Success("value");

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
            () => result.Match(value => $"success:{value}", null!));

        Assert.Equal("onFailure", exception.ParamName);
    }
}
