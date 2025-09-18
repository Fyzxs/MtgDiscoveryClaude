using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Shared.Abstractions.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Shared.Abstractions.Tests.Actions;

[TestClass]
public sealed class ValidatorContainerTests
{
    private sealed class TestValidatorContainer : ValidatorContainer<TestTarget>
    {
        public TestValidatorContainer(params IValidator<TestTarget>[] validators) : base(validators) { }
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithNoValidators_ReturnsTrue()
    {
        // Arrange
        TestTarget target = new();
        TestValidatorContainer subject = new();

        // Act
        bool actual = await subject.IsValid(target).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithSingleValidValidator_ReturnsTrue()
    {
        // Arrange
        TestTarget target = new();
        ValidatorFake validator = new() { IsValidResult = true };
        TestValidatorContainer subject = new(validator);

        // Act
        bool actual = await subject.IsValid(target).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
        validator.IsValidInvokeCount.Should().Be(1);
        validator.IsValidInput.Should().BeSameAs(target);
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithFirstValidatorInvalid_ReturnsFalse()
    {
        // Arrange
        TestTarget target = new();
        ValidatorFake validator1 = new() { IsValidResult = false };
        ValidatorFake validator2 = new() { IsValidResult = true };
        TestValidatorContainer subject = new(validator1, validator2);

        // Act
        bool actual = await subject.IsValid(target).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
        validator1.IsValidInvokeCount.Should().Be(1);
        validator2.IsValidInvokeCount.Should().Be(0); // Should not be called
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithAllValidatorsValid_ReturnsTrue()
    {
        // Arrange
        TestTarget target = new();
        ValidatorFake validator1 = new() { IsValidResult = true };
        ValidatorFake validator2 = new() { IsValidResult = true };
        ValidatorFake validator3 = new() { IsValidResult = true };
        TestValidatorContainer subject = new(validator1, validator2, validator3);

        // Act
        bool actual = await subject.IsValid(target).ConfigureAwait(false);

        // Assert
        actual.Should().BeTrue();
        validator1.IsValidInvokeCount.Should().Be(1);
        validator2.IsValidInvokeCount.Should().Be(1);
        validator3.IsValidInvokeCount.Should().Be(1);
    }

    [TestMethod, TestCategory("unit")]
    public async Task IsValid_WithMiddleValidatorInvalid_ReturnsFalse()
    {
        // Arrange
        TestTarget target = new();
        ValidatorFake validator1 = new() { IsValidResult = true };
        ValidatorFake validator2 = new() { IsValidResult = false };
        ValidatorFake validator3 = new() { IsValidResult = true };
        TestValidatorContainer subject = new(validator1, validator2, validator3);

        // Act
        bool actual = await subject.IsValid(target).ConfigureAwait(false);

        // Assert
        actual.Should().BeFalse();
        validator1.IsValidInvokeCount.Should().Be(1);
        validator2.IsValidInvokeCount.Should().Be(1);
        validator3.IsValidInvokeCount.Should().Be(0); // Should not be called
    }

    private sealed class TestTarget
    {
        public string Value { get; set; } = "";
    }

    private sealed class ValidatorFake : IValidator<TestTarget>
    {
        public bool IsValidResult { get; init; }
        public int IsValidInvokeCount { get; private set; }
        public TestTarget IsValidInput { get; private set; } = default!;

        public Task<bool> IsValid(TestTarget target)
        {
            IsValidInvokeCount++;
            IsValidInput = target;
            return Task.FromResult(IsValidResult);
        }
    }
}