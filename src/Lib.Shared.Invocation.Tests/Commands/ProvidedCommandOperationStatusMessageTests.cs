using Lib.Shared.Invocation.Commands;

namespace Lib.Shared.Invocation.Tests.Commands;

[TestClass]
public sealed class ProvidedCommandOperationStatusMessageTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_StoresMessage()
    {
        // Arrange
        string expectedMessage = "Test message";

        // Act
        ProvidedCommandOperationStatusMessage subject = new(expectedMessage);

        // Assert
        subject.AsSystemType().Should().Be(expectedMessage);
    }

    [TestMethod, TestCategory("unit")]
    public void AsSystemType_ReturnsProvidedMessage()
    {
        // Arrange
        string expectedMessage = "Another message";
        ProvidedCommandOperationStatusMessage subject = new(expectedMessage);

        // Act
        string actual = subject.AsSystemType();

        // Assert
        actual.Should().Be(expectedMessage);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithEmptyString_StoresEmptyString()
    {
        // Arrange & Act
        ProvidedCommandOperationStatusMessage subject = new(string.Empty);

        // Assert
        subject.AsSystemType().Should().Be(string.Empty);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithWhitespace_StoresWhitespace()
    {
        // Arrange
        string whitespace = "   ";

        // Act
        ProvidedCommandOperationStatusMessage subject = new(whitespace);

        // Assert
        subject.AsSystemType().Should().Be(whitespace);
    }
}
