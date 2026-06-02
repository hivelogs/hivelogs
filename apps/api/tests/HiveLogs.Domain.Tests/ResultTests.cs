using FluentAssertions;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldHaveNoError()
    {
        var result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_ShouldContainError()
    {
        var error = Error.NotFound("general.not_found", "Not found");
        var result = Result.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void GenericSuccess_ShouldExposeValue()
    {
        var result = Result<int>.Success(42);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void GenericFailure_ShouldNotThrowWhenCheckingError()
    {
        var error = Error.Validation("general.validation", "Invalid");
        var result = Result<int>.Failure(error);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);

        var act = () => _ = result.Value;
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ExpectedError_ShouldNotUseException()
    {
        var result = Result.Failure(GeneralErrors.NotFound);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("general.not_found");
    }
}
