using Lib.Shared.Invocation.Commands;

namespace Lib.Shared.Invocation.Tests.Commands;

[TestClass]
public sealed class CommandOperationStatusMessageTests
{
    private sealed class TestCommandOperationStatusMessage : CommandOperationStatusMessage
    {
        private readonly string _value;

        public TestCommandOperationStatusMessage(string value) => _value = value;

        public override string AsSystemType() => _value;
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_WhenCalled_ReturnsExpectedValue()
    {
        // Arrange
        string expectedValue = "Test message";
        TestCommandOperationStatusMessage subject = new(expectedValue);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be(expectedValue);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WhenCalled_CreatesInstance()
    {
        // Arrange & Act
        TestCommandOperationStatusMessage subject = new("Test");

        // Assert
        subject.Should().NotBeNull();
    }
}
