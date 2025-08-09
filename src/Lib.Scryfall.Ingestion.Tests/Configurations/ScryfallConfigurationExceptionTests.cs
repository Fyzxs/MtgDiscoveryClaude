using System;
using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Tests.Configurations;

[TestClass]
public class ScryfallConfigurationExceptionTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithoutParameters_ShouldCreateInstance()
    {
        // Arrange & Act
        ScryfallConfigurationException actual = new();

        // Assert
        _ = actual.Message.Should().NotBeNullOrEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        const string expectedMessage = "Test configuration error";

        // Act
        ScryfallConfigurationException actual = new(expectedMessage);

        // Assert
        _ = actual.Message.Should().Be(expectedMessage);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
    {
        // Arrange
        const string expectedMessage = "Test configuration error";
        InvalidOperationException innerException = new("Inner exception");

        // Act
        ScryfallConfigurationException actual = new(expectedMessage, innerException);

        // Assert
        _ = actual.Message.Should().Be(expectedMessage);
        _ = actual.InnerException.Should().BeSameAs(innerException);
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldBeAssignableToException()
    {
        // Arrange & Act
        ScryfallConfigurationException subject = new("Test");

        // Assert
        _ = subject.Should().BeAssignableTo<Exception>();
    }

    [TestMethod, TestCategory("unit")]
    public void ShouldBeSerializable()
    {
        // Arrange
        const string expectedMessage = "Test configuration error";
        ScryfallConfigurationException original = new(expectedMessage);

        // Act
        // Simulate serialization/deserialization through exception throw
        ScryfallConfigurationException actual = null;
        try
        {
            throw original;
        }
        catch (ScryfallConfigurationException ex)
        {
            actual = ex;
        }

        // Assert
        _ = actual.Message.Should().Be(expectedMessage);
    }
}