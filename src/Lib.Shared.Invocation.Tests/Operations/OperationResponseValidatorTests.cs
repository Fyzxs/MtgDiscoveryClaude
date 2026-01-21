using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Tests.Operations;

[TestClass]
public sealed class OperationResponseValidatorTests
{
    private sealed class TestOperationResponseMessage : OperationResponseMessage
    {
        private readonly string _value;

        public TestOperationResponseMessage(string value) => _value = value;

        public override string AsSystemType() => _value;
    }

    private sealed class TestOperationResponseValidator : OperationResponseValidator<string, int>
    {
        public TestOperationResponseValidator(IValidator<string> validator, OperationResponseMessage message)
            : base(validator, message)
        {
        }
    }

    private sealed class ValidatorFake : IValidator<string>
    {
        public bool IsValidResult { get; init; }
        public int IsValidInvokeCount { get; private set; }
        public string IsValidInput { get; private set; } = default!;

        public Task<bool> IsValid(string entity)
        {
            IsValidInvokeCount++;
            IsValidInput = entity;
            return Task.FromResult(IsValidResult);
        }
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithValidItem_ReturnsValidResult()
    {
        // Arrange
        string testItem = "valid item";
        TestOperationResponseMessage message = new("error message");
        ValidatorFake validator = new() { IsValidResult = true };
        TestOperationResponseValidator subject = new(validator, message);

        // Act
        IValidatorActionResult<IOperationResponse<int>> actual = await subject.Validate(testItem).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeTrue();
        validator.IsValidInvokeCount.Should().Be(1);
        validator.IsValidInput.Should().Be(testItem);
    }

    [TestMethod, TestCategory("unit")]
    public async Task Validate_WithInvalidItem_ReturnsFailureResult()
    {
        // Arrange
        string testItem = "invalid item";
        string expectedMessage = "Validation failed";
        TestOperationResponseMessage message = new(expectedMessage);
        ValidatorFake validator = new() { IsValidResult = false };
        TestOperationResponseValidator subject = new(validator, message);

        // Act
        IValidatorActionResult<IOperationResponse<int>> actual = await subject.Validate(testItem).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeFalse();
        IOperationResponse<int> failureResponse = actual.FailureStatus();
        failureResponse.Should().NotBeNull();
        failureResponse.IsFailure.Should().BeTrue();
        failureResponse.OuterException.StatusMessage.Should().Be(expectedMessage);
        validator.IsValidInvokeCount.Should().Be(1);
        validator.IsValidInput.Should().Be(testItem);
    }
}
