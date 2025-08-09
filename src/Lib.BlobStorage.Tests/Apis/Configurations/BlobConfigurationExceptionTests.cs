using System;
using Lib.BlobStorage.Apis.Configurations;

namespace Lib.BlobStorage.Tests.Apis.Configurations;

[TestClass]
public sealed class BlobConfigurationExceptionTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_ShouldInitializeMessage_WhenMessageProvided()
    {
        // Arrange
        const string ExpectedMessage = "Test message";

        // Act
        BlobConfigurationException actual = new(ExpectedMessage);

        // Assert
        actual.Message.Should().Be(ExpectedMessage);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_ShouldInitializeMessageAndInnerException_WhenBothProvided()
    {
        // Arrange
        const string ExpectedMessage = "Test message";
#pragma warning disable CA2201 // Do not raise reserved exception types
        Exception expectedInnerException = new("Inner exception");
#pragma warning restore CA2201 // Do not raise reserved exception types

        // Act
        BlobConfigurationException actual = new(ExpectedMessage, expectedInnerException);

        // Assert
        actual.Message.Should().Be(ExpectedMessage);
        actual.InnerException.Should().Be(expectedInnerException);
    }

    [TestMethod, TestCategory("unit")]
    public void DefaultConstructor_ShouldInitializeEmptyMessage()
    {
        // Act
        BlobConfigurationException actual = new();

        // Assert
        actual.Message.Should().Be("Exception of type 'Lib.BlobStorage.Apis.Configurations.BlobConfigurationException' was thrown.");
    }
}
