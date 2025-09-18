using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class FilterContainerTests
{
    private sealed class TestFilterContainer : FilterContainer<TestTarget, TestResult>
    {
        public TestFilterContainer(params IFilter<TestTarget, TestResult>[] filters) : base(filters) { }
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithNoFilters_ReturnsFalse()
    {
        // Arrange
        TestTarget target = new();
        TestResult result = new();
        TestFilterContainer subject = new();

        // Act
        bool actual = await subject.IsFilteredOut(target, result).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithSingleNotFilteredFilter_ReturnsFalse()
    {
        // Arrange
        TestTarget target = new();
        TestResult result = new();
        FilterFake filter = new() { IsFilteredOutResult = false };
        TestFilterContainer subject = new(filter);

        // Act
        bool actual = await subject.IsFilteredOut(target, result).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
        filter.IsFilteredOutInvokeCount.Should().Be(1);
        filter.IsFilteredOutInput1.Should().BeSameAs(target);
        filter.IsFilteredOutInput2.Should().BeSameAs(result);
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithFirstFilterFiltered_ReturnsTrue()
    {
        // Arrange
        TestTarget target = new();
        TestResult result = new();
        FilterFake filter1 = new() { IsFilteredOutResult = true };
        FilterFake filter2 = new() { IsFilteredOutResult = false };
        TestFilterContainer subject = new(filter1, filter2);

        // Act
        bool actual = await subject.IsFilteredOut(target, result).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
        filter1.IsFilteredOutInvokeCount.Should().Be(1);
        filter2.IsFilteredOutInvokeCount.Should().Be(0); // Should not be called
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsFilteredOut_WithAllFiltersNotFiltered_ReturnsFalse()
    {
        // Arrange
        TestTarget target = new();
        TestResult result = new();
        FilterFake filter1 = new() { IsFilteredOutResult = false };
        FilterFake filter2 = new() { IsFilteredOutResult = false };
        FilterFake filter3 = new() { IsFilteredOutResult = false };
        TestFilterContainer subject = new(filter1, filter2, filter3);

        // Act
        bool actual = await subject.IsFilteredOut(target, result).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
        filter1.IsFilteredOutInvokeCount.Should().Be(1);
        filter2.IsFilteredOutInvokeCount.Should().Be(1);
        filter3.IsFilteredOutInvokeCount.Should().Be(1);
    }

    private sealed class TestTarget
    {
        public string Value { get; set; } = "";
    }

    private sealed class TestResult
    {
        public string Value { get; set; } = "";
    }

    private sealed class FilterFake : IFilter<TestTarget, TestResult>
    {
        public bool IsFilteredOutResult { get; init; }
        public int IsFilteredOutInvokeCount { get; private set; }
        public TestTarget IsFilteredOutInput1 { get; private set; } = default!;
        public TestResult IsFilteredOutInput2 { get; private set; } = default!;

        public Task<bool> IsFilteredOut(TestTarget item1, TestResult item2)
        {
            IsFilteredOutInvokeCount++;
            IsFilteredOutInput1 = item1;
            IsFilteredOutInput2 = item2;
            return Task.FromResult(IsFilteredOutResult);
        }
    }
}