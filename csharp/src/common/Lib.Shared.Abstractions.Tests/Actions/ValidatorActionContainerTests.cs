using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class ValidatorActionContainerTests
{
    private sealed class TestValidatorActionContainer : ValidatorActionContainer<TestTarget, TestResult>
    {
        public TestValidatorActionContainer(params IValidatorAction<TestTarget, TestResult>[] actions) : base(actions) { }
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithNoActions_ReturnsValidResult()
    {
        // Arrange
        TestTarget target = new();
        TestValidatorActionContainer subject = new();

        // Act
        IValidatorActionResult<TestResult> actual = await subject.Validate(target).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithSingleValidAction_ReturnsValidResult()
    {
        // Arrange
        TestTarget target = new();
        ValidatorActionFake action = new() { ValidateResult = new ValidatorActionResultFake { IsValidValue = true } };
        TestValidatorActionContainer subject = new(action);

        // Act
        IValidatorActionResult<TestResult> actual = await subject.Validate(target).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
        action.ValidateInvokeCount.Should().Be(1);
        action.ValidateInput.Should().BeSameAs(target);
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithFirstActionInvalid_ReturnsInvalidResult()
    {
        // Arrange
        TestTarget target = new();
        TestResult failureResult = new() { Value = "Failed" };
        ValidatorActionFake action1 = new()
        {
            ValidateResult = new ValidatorActionResultFake
            {
                IsValidValue = false,
                FailureStatusValue = failureResult
            }
        };
        ValidatorActionFake action2 = new() { ValidateResult = new ValidatorActionResultFake { IsValidValue = true } };
        TestValidatorActionContainer subject = new(action1, action2);

        // Act
        IValidatorActionResult<TestResult> actual = await subject.Validate(target).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeFalse();
        actual.FailureStatus().Should().BeSameAs(failureResult);
        action1.ValidateInvokeCount.Should().Be(1);
        action2.ValidateInvokeCount.Should().Be(0); // Should not be called
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithAllActionsValid_ReturnsValidResult()
    {
        // Arrange
        TestTarget target = new();
        ValidatorActionFake action1 = new() { ValidateResult = new ValidatorActionResultFake { IsValidValue = true } };
        ValidatorActionFake action2 = new() { ValidateResult = new ValidatorActionResultFake { IsValidValue = true } };
        ValidatorActionFake action3 = new() { ValidateResult = new ValidatorActionResultFake { IsValidValue = true } };
        TestValidatorActionContainer subject = new(action1, action2, action3);

        // Act
        IValidatorActionResult<TestResult> actual = await subject.Validate(target).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
        action1.ValidateInvokeCount.Should().Be(1);
        action2.ValidateInvokeCount.Should().Be(1);
        action3.ValidateInvokeCount.Should().Be(1);
    }

    private sealed class TestTarget
    {
        public string Value { get; set; } = "";
    }

    private sealed class TestResult
    {
        public string Value { get; set; } = "";
    }

    private sealed class ValidatorActionFake : IValidatorAction<TestTarget, TestResult>
    {
        public IValidatorActionResult<TestResult> ValidateResult { get; init; } = new ValidatorActionResultFake();
        public int ValidateInvokeCount { get; private set; }
        public TestTarget ValidateInput { get; private set; } = default!;

        public Task<IValidatorActionResult<TestResult>> Validate(TestTarget target)
        {
            ValidateInvokeCount++;
            ValidateInput = target;
            return Task.FromResult(ValidateResult);
        }
    }

    private sealed class ValidatorActionResultFake : IValidatorActionResult<TestResult>
    {
        public bool IsValidValue { get; init; }
        public TestResult FailureStatusValue { get; init; } = new();

        public bool IsValid() => IsValidValue;
        public TestResult FailureStatus() => FailureStatusValue;
    }
}