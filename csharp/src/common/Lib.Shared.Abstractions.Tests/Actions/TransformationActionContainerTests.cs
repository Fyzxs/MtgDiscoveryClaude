using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class TransformationActionContainerTests
{
    private sealed class TestTransformationActionContainer : TransformationActionContainer<TestTarget>
    {
        public TestTransformationActionContainer(params ITransformationAction<TestTarget>[] actions) : base(actions) { }
    }

    [TestMethod, TestCategory("unit")]
    public void Transformation_WithNoActions_CompletesSuccessfully()
    {
        // Arrange
        TestTarget target = new() { Value = "Original" };
        TestTransformationActionContainer subject = new();

        // Act
        subject.Transformation(target);

        // Assert
        target.Value.Should().Be("Original");
    }

    [TestMethod, TestCategory("unit")]
    public void Transformation_WithSingleAction_TransformsTarget()
    {
        // Arrange
        TestTarget target = new() { Value = "Original" };
        TransformationActionFake action = new();
        TestTransformationActionContainer subject = new(action);

        // Act
        subject.Transformation(target);

        // Assert
        action.TransformationInvokeCount.Should().Be(1);
        action.TransformationInput.Should().BeSameAs(target);
    }

    [TestMethod, TestCategory("unit")]
    public void Transformation_WithMultipleActions_CallsAllActionsInOrder()
    {
        // Arrange
        TestTarget target = new() { Value = "Original" };

        TransformationActionFake action1 = new();
        TransformationActionFake action2 = new();
        TransformationActionFake action3 = new();

        TestTransformationActionContainer subject = new(action1, action2, action3);

        // Act
        subject.Transformation(target);

        // Assert
        action1.TransformationInvokeCount.Should().Be(1);
        action1.TransformationInput.Should().BeSameAs(target);
        action2.TransformationInvokeCount.Should().Be(1);
        action2.TransformationInput.Should().BeSameAs(target);
        action3.TransformationInvokeCount.Should().Be(1);
        action3.TransformationInput.Should().BeSameAs(target);
    }

    private sealed class TestTarget
    {
        public string Value { get; set; } = "";
    }

    private sealed class TransformationActionFake : ITransformationAction<TestTarget>
    {
        public int TransformationInvokeCount { get; private set; }
        public TestTarget TransformationInput { get; private set; } = default!;

        public void Transformation(TestTarget target)
        {
            TransformationInvokeCount++;
            TransformationInput = target;
        }
    }
}