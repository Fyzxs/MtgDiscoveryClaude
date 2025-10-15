using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Requests;

namespace Lib.Shared.Invocation.Tests.Requests;

[TestClass]
public sealed class OperationStatusResponseValidatorTests
{
    private sealed class TestOperationStatusResponseValidator : OperationStatusResponseValidator<string, int>
    {
        public TestOperationStatusResponseValidator(IValidator<string> validator, string message)
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
        ValidatorFake validator = new() { IsValidResult = true };
        TestOperationStatusResponseValidator subject = new(validator, "error message");

        // Act
        IValidatorActionResult<OperationResponse<int>> actual = await subject.Validate(testItem).ConfigureAwait(false);

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
        ValidatorFake validator = new() { IsValidResult = false };
        TestOperationStatusResponseValidator subject = new(validator, expectedMessage);

        // Act
        IValidatorActionResult<OperationResponse<int>> actual = await subject.Validate(testItem).ConfigureAwait(false);

        // Assert
        actual.IsValid().Should().BeFalse();
        OperationResponse<int> failureResponse = actual.FailureStatus();
        failureResponse.Should().NotBeNull();
        failureResponse.IsFailure.Should().BeTrue();
        failureResponse.OuterException.StatusMessage.Should().Be(expectedMessage);
        validator.IsValidInvokeCount.Should().Be(1);
        validator.IsValidInput.Should().Be(testItem);
    }
}
