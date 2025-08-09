using System.Net;
using Lib.Cosmos.Operators;
using Lib.Cosmos.Tests.Fakes;

namespace Lib.Cosmos.Tests.Operators;

[TestClass]
public sealed class ItemOpResponseTests
{
    [TestMethod, TestCategory("unit")]
    public void Value_ShouldReturnResourceFromItemResponse()
    {
        // Arrange
        TestItem expectedItem = new() { Id = "testId", Name = "testName" };
        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = expectedItem,
            StatusCodeResult = HttpStatusCode.OK
        };

        ItemOpResponse<TestItem> subject = new(itemResponseFake);

        // Act
        TestItem result = subject.Value;

        // Assert
        result.Should().Be(expectedItem);
    }

    [TestMethod, TestCategory("unit")]
    public void StatusCode_ShouldReturnStatusCodeFromItemResponse()
    {
        // Arrange
        const HttpStatusCode ExpectedStatusCode = HttpStatusCode.Created;
        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = new TestItem(),
            StatusCodeResult = ExpectedStatusCode
        };

        ItemOpResponse<TestItem> subject = new(itemResponseFake);

        // Act
        HttpStatusCode result = subject.StatusCode;

        // Assert
        result.Should().Be(ExpectedStatusCode);
    }

    [TestMethod, TestCategory("unit")]
    public void Constructor_WithItemResponse_ShouldCreateInstance()
    {
        // Arrange
        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = new TestItem(),
            StatusCodeResult = HttpStatusCode.OK
        };

        // Act
        ItemOpResponse<TestItem> subject = new(itemResponseFake);

        // Assert
        subject.Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void Value_WithNullResource_ShouldReturnNull()
    {
        // Arrange
        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = null,
            StatusCodeResult = HttpStatusCode.NotFound
        };

        ItemOpResponse<TestItem> subject = new(itemResponseFake);

        // Act
        TestItem result = subject.Value;

        // Assert
        result.Should().BeNull();
    }

    [TestMethod, TestCategory("unit")]
    public void StatusCode_WithVariousHttpStatusCodes_ShouldReturnCorrectCode()
    {
        // Arrange
        const HttpStatusCode ExpectedStatusCode = HttpStatusCode.TooManyRequests;
        ItemResponseFake<TestItem> itemResponseFake = new()
        {
            ResourceResult = new TestItem(),
            StatusCodeResult = ExpectedStatusCode
        };

        ItemOpResponse<TestItem> subject = new(itemResponseFake);

        // Act
        HttpStatusCode result = subject.StatusCode;

        // Assert
        result.Should().Be(ExpectedStatusCode);
    }

    private sealed class TestItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }
}
