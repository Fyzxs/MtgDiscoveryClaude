using System.Collections.Generic;
using System.Net;
using Lib.Cosmos.Operators;

namespace Lib.Cosmos.Tests.Operators;

[TestClass]
public sealed class ProvidedOpResponseTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithValueAndStatusCode_ShouldSetProperties()
    {
        // Arrange
        TestItem expectedItem = new() { Id = "1", Name = "Test" };
        const HttpStatusCode ExpectedStatusCode = HttpStatusCode.OK;

        // Act
        ProvidedOpResponse<TestItem> actual = new(expectedItem, ExpectedStatusCode);

        // Assert
        actual.Value.Should().Be(expectedItem);
        actual.StatusCode.Should().Be(ExpectedStatusCode);
    }

    [TestMethod, TestCategory("unit")]
    public void Value_ShouldReturnProvidedValue()
    {
        // Arrange
        TestItem expectedItem = new() { Id = "123", Name = "Test Item" };
        ProvidedOpResponse<TestItem> subject = new(expectedItem, HttpStatusCode.Created);

        // Act
        TestItem actual = subject.Value;

        // Assert
        actual.Should().Be(expectedItem);
    }

    [TestMethod, TestCategory("unit")]
    public void StatusCode_ShouldReturnProvidedStatusCode()
    {
        // Arrange
        const HttpStatusCode ExpectedStatusCode = HttpStatusCode.NotFound;
        ProvidedOpResponse<TestItem> subject = new(new TestItem(), ExpectedStatusCode);

        // Act
        HttpStatusCode actual = subject.StatusCode;

        // Assert
        actual.Should().Be(ExpectedStatusCode);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithNullValue_ShouldAllowNull()
    {
        // Arrange
        TestItem nullItem = null;
        const HttpStatusCode StatusCode = HttpStatusCode.NotFound;

        // Act
        ProvidedOpResponse<TestItem> actual = new(nullItem, StatusCode);

        // Assert
        actual.Value.Should().BeNull();
        actual.StatusCode.Should().Be(StatusCode);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithStringValue_ShouldWorkWithDifferentTypes()
    {
        // Arrange
        const string ExpectedValue = "Test String Value";
        const HttpStatusCode ExpectedStatusCode = HttpStatusCode.Accepted;

        // Act
        ProvidedOpResponse<string> actual = new(ExpectedValue, ExpectedStatusCode);

        // Assert
        actual.Value.Should().Be(ExpectedValue);
        actual.StatusCode.Should().Be(ExpectedStatusCode);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithCollectionValue_ShouldWorkWithCollections()
    {
        // Arrange
        List<int> expectedCollection = [1, 2, 3, 4, 5];
        const HttpStatusCode ExpectedStatusCode = HttpStatusCode.OK;

        // Act
        ProvidedOpResponse<List<int>> actual = new(expectedCollection, ExpectedStatusCode);

        // Assert
        actual.Value.Should().BeEquivalentTo(expectedCollection);
        actual.StatusCode.Should().Be(ExpectedStatusCode);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithVariousStatusCodes_ShouldAcceptAllHttpStatusCodes()
    {
        // Arrange
        TestItem item = new() { Id = "1" };
        HttpStatusCode[] statusCodes =
        [
            HttpStatusCode.OK,
            HttpStatusCode.Created,
            HttpStatusCode.Accepted,
            HttpStatusCode.NoContent,
            HttpStatusCode.BadRequest,
            HttpStatusCode.NotFound,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.ServiceUnavailable
        ];

        foreach (HttpStatusCode statusCode in statusCodes)
        {
            // Act
            ProvidedOpResponse<TestItem> actual = new(item, statusCode);

            // Assert
            actual.StatusCode.Should().Be(statusCode);
        }
    }

    private sealed class TestItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }
}
