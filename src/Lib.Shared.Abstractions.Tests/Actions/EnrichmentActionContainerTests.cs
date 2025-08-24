using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class EnrichmentActionContainerTests
{
    private sealed class TestEnrichmentActionContainer : EnrichmentActionContainer<TestTarget>
    {
        public TestEnrichmentActionContainer(params IEnrichmentAction<TestTarget>[] actions) : base(actions) { }
    }

    [TestMethod, TestCategory("unit")]
    public async Task Enrich_WithNoActions_CompletesSuccessfully()
    {
        // Arrange
        TestTarget target = new();
        TestEnrichmentActionContainer subject = new();

        // Act
        await subject.Enrich(target).ConfigureAwait(false);

        // Assert
        // Should complete without throwing
    }

    [TestMethod, TestCategory("unit")]
    public async Task Enrich_WithSingleAction_CallsAction()
    {
        // Arrange
        TestTarget target = new();
        FakeEnrichmentAction action = new();
        TestEnrichmentActionContainer subject = new(action);

        // Act
        await subject.Enrich(target).ConfigureAwait(false);

        // Assert
        action.EnrichInvokeCount.Should().Be(1);
        action.EnrichInput.Should().BeSameAs(target);
    }

    [TestMethod, TestCategory("unit")]
    public async Task Enrich_WithMultipleActions_CallsAllActionsInOrder()
    {
        // Arrange
        TestTarget target = new();
        FakeEnrichmentAction action1 = new();
        FakeEnrichmentAction action2 = new();
        FakeEnrichmentAction action3 = new();
        TestEnrichmentActionContainer subject = new(action1, action2, action3);

        // Act
        await subject.Enrich(target).ConfigureAwait(false);

        // Assert
        action1.EnrichInvokeCount.Should().Be(1);
        action2.EnrichInvokeCount.Should().Be(1);
        action3.EnrichInvokeCount.Should().Be(1);
        action1.EnrichInput.Should().BeSameAs(target);
        action2.EnrichInput.Should().BeSameAs(target);
        action3.EnrichInput.Should().BeSameAs(target);
    }

    private sealed class TestTarget
    {
        public string Value { get; set; } = "";
    }

    private sealed class FakeEnrichmentAction : IEnrichmentAction<TestTarget>
    {
        public int EnrichInvokeCount { get; private set; }
        public TestTarget EnrichInput { get; private set; } = default!;

        public Task Enrich(TestTarget target)
        {
            EnrichInvokeCount++;
            EnrichInput = target;
            return Task.CompletedTask;
        }
    }
}