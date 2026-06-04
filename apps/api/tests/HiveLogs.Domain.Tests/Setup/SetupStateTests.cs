using FluentAssertions;
using HiveLogs.Domain.Setup;

namespace HiveLogs.Domain.Tests.Setup;

public class SetupStateTests
{
    [Fact]
    public void CreatePending_ShouldReturnSetupRequired()
    {
        var state = SetupState.CreatePending();

        state.IsCompleted.Should().BeFalse();
        state.GetStatus().Should().Be(SetupStatus.SetupRequired);
    }

    [Fact]
    public void CreatePending_ShouldUseSingletonId()
    {
        var first = SetupState.CreatePending();
        var second = SetupState.CreatePending();

        first.Id.Should().Be(SetupState.SingletonId);
        second.Id.Should().Be(SetupState.SingletonId);
        first.Id.Should().Be(second.Id);
    }

    [Fact]
    public void MarkCompleted_ShouldReturnConfigured()
    {
        var state = SetupState.CreatePending();
        var completedAt = DateTimeOffset.Parse("2026-06-04T12:00:00Z");

        state.MarkCompleted(completedAt);

        state.IsCompleted.Should().BeTrue();
        state.CompletedAt.Should().Be(completedAt);
        state.GetStatus().Should().Be(SetupStatus.Configured);
    }
}
