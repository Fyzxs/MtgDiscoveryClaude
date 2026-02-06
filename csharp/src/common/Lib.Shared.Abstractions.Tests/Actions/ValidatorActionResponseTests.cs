using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Universal.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class ValidatorActionResponseTests
{
    [TestMethod, TestCategory("unit")]
    public async Task TryIsValid_ValidItem_ReturnsSuccess()
    {
        // Arrange
        ValidatorActionFake validator = new(isValid: true);
        TestValidatorActionResponse subject = new(validator);

        // Act
        IAsyncTryOut<string> actual = await subject.TryIsValid("valid input").ConfigureAwait(false);

        // Assert
        actual.IsSuccess().Should().BeTrue();
        actual.IsFailure().Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public async Task TryIsValid_InvalidItem_ReturnsFailure()
    {
        // Arrange
        ValidatorActionFake validator = new(isValid: false, failureStatus: "validation error");
        TestValidatorActionResponse subject = new(validator);

        // Act
        IAsyncTryOut<string> actual = await subject.TryIsValid("invalid input").ConfigureAwait(false);

        // Assert
        actual.IsSuccess().Should().BeFalse();
        actual.IsFailure().Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task TryIsValid_InvalidItem_CallsValueMethod()
    {
        // Arrange
        ValidatorActionFake validator = new(isValid: false, failureStatus: "error");
        TestValidatorActionResponse subject = new(validator);

        // Act
        _ = await subject.TryIsValid("input").ConfigureAwait(false);

        // Assert
        subject.ValueCallCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task TryIsValid_ValidItem_DoesNotCallValueMethod()
    {
        // Arrange
        ValidatorActionFake validator = new(isValid: true);
        TestValidatorActionResponse subject = new(validator);

        // Act
        _ = await subject.TryIsValid("input").ConfigureAwait(false);

        // Assert
        subject.ValueCallCount.Should().Be(0);
    }

    [TestMethod, TestCategory("unit")]
    public async Task TryIsValid_CallsValidatorWithCorrectInput()
    {
        // Arrange
        ValidatorActionFake validator = new(isValid: true);
        TestValidatorActionResponse subject = new(validator);

        // Act
        _ = await subject.TryIsValid("test-input").ConfigureAwait(false);

        // Assert
        validator.ValidateInput.Should().Be("test-input");
        validator.ValidateCallCount.Should().Be(1);
    }

    private sealed class TestValidatorActionResponse : ValidatorActionResponse<string, string, string>
    {
        public int ValueCallCount { get; private set; }

        public TestValidatorActionResponse(IValidatorAction<string, string> validator) : base(validator) { }

        protected override string Value(IValidatorActionResult<string> validatorResult)
        {
            ValueCallCount++;
            return $"Mapped: {validatorResult.FailureStatus()}";
        }
    }

    private sealed class ValidatorActionFake : IValidatorAction<string, string>
    {
        private readonly bool _isValid;
        private readonly string _failureStatus;

        public string ValidateInput { get; private set; } = default!;
        public int ValidateCallCount { get; private set; }

        public ValidatorActionFake(bool isValid, string failureStatus = "")
        {
            _isValid = isValid;
            _failureStatus = failureStatus;
        }

        public Task<IValidatorActionResult<string>> Validate(string item)
        {
            ValidateCallCount++;
            ValidateInput = item;
            IValidatorActionResult<string> result = _isValid
                ? new ValidActionResultFake()
                : new InvalidActionResultFake(_failureStatus);
            return Task.FromResult(result);
        }
    }

    private sealed class ValidActionResultFake : IValidatorActionResult<string>
    {
        public bool IsValid() => true;
        public string FailureStatus() => throw new System.InvalidOperationException();
    }

    private sealed class InvalidActionResultFake : IValidatorActionResult<string>
    {
        private readonly string _failureStatus;
        public InvalidActionResultFake(string failureStatus) => _failureStatus = failureStatus;
        public bool IsValid() => false;
        public string FailureStatus() => _failureStatus;
    }
}
