using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class FilterActionContainerTests
{
    private sealed class TestFilterActionContainer : FilterActionContainer<TestTarget, TestResult>
    {
        public TestFilterActionContainer(params IFilterAction<TestTarget, TestResult>[] actions) : base(actions) { }
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithNoActions_ReturnsNotFilteredResult()
    {
        // Arrange
        TestTarget target = new();
        TestFilterActionContainer subject = new();

        // Act
        IFilterActionResult<TestResult> actual = await subject.IsFilteredOut(target).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFilteredOut().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithSingleNotFilteredAction_ReturnsNotFilteredResult()
    {
        // Arrange
        TestTarget target = new();
        FilterActionFake action = new() { FilterResult = new FilterActionResultFake { IsFilteredOutValue = false } };
        TestFilterActionContainer subject = new(action);

        // Act
        IFilterActionResult<TestResult> actual = await subject.IsFilteredOut(target).ConfigureAwait(false);

        // Assert
        actual.IsFilteredOut().Should().BeFalse();
        action.FilterInvokeCount.Should().Be(1);
        action.FilterInput.Should().BeSameAs(target);
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithFirstActionFiltered_ReturnsFilteredResult()
    {
        // Arrange
        TestTarget target = new();
        TestResult failureResult = new() { Value = "Failed" };
        FilterActionFake action1 = new()
        {
            FilterResult = new FilterActionResultFake
            {
                IsFilteredOutValue = true,
                FailureStatusValue = failureResult
            }
        };
        FilterActionFake action2 = new() { FilterResult = new FilterActionResultFake { IsFilteredOutValue = false } };
        TestFilterActionContainer subject = new(action1, action2);

        // Act
        IFilterActionResult<TestResult> actual = await subject.IsFilteredOut(target).ConfigureAwait(false);

        // Assert
        actual.IsFilteredOut().Should().BeTrue();
        actual.FailureStatus().Should().BeSameAs(failureResult);
        action1.FilterInvokeCount.Should().Be(1);
        action2.FilterInvokeCount.Should().Be(0); // Should not be called
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithAllActionsNotFiltered_ReturnsNotFilteredResult()
    {
        // Arrange
        TestTarget target = new();
        FilterActionFake action1 = new() { FilterResult = new FilterActionResultFake { IsFilteredOutValue = false } };
        FilterActionFake action2 = new() { FilterResult = new FilterActionResultFake { IsFilteredOutValue = false } };
        FilterActionFake action3 = new() { FilterResult = new FilterActionResultFake { IsFilteredOutValue = false } };
        TestFilterActionContainer subject = new(action1, action2, action3);

        // Act
        IFilterActionResult<TestResult> actual = await subject.IsFilteredOut(target).ConfigureAwait(false);

        // Assert
        actual.IsFilteredOut().Should().BeFalse();
        action1.FilterInvokeCount.Should().Be(1);
        action2.FilterInvokeCount.Should().Be(1);
        action3.FilterInvokeCount.Should().Be(1);
    }

    private sealed class TestTarget
    {
        public string Value { get; set; } = "";
    }

    private sealed class TestResult
    {
        public string Value { get; set; } = "";
    }

    private sealed class FilterActionFake : IFilterAction<TestTarget, TestResult>
    {
        public IFilterActionResult<TestResult> FilterResult { get; init; } = new FilterActionResultFake();
        public int FilterInvokeCount { get; private set; }
        public TestTarget FilterInput { get; private set; } = default!;

        public Task<IFilterActionResult<TestResult>> IsFilteredOut(TestTarget item)
        {
            FilterInvokeCount++;
            FilterInput = item;
            return Task.FromResult(FilterResult);
        }
    }

    private sealed class FilterActionResultFake : IFilterActionResult<TestResult>
    {
        public bool IsFilteredOutValue { get; init; }
        public TestResult FailureStatusValue { get; init; } = new();

        public bool IsFilteredOut() => IsFilteredOutValue;
        public TestResult FailureStatus() => FailureStatusValue;
    }
}