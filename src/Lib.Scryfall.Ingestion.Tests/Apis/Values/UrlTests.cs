using System;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Tests.Apis.Values;

[TestClass]
public sealed class UrlTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_ValidUrl_ReturnsUri()
    {
        // Arrange
        const string urlString = "https://api.scryfall.com/sets";
        Url url = new(urlString);

        // Act
        Uri actual = url.AsSystemType();

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.AbsoluteUri.Should().Be(urlString);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Constructor_ValidUrl_CreatesInstance()
    {
        // Arrange
        const string urlString = "https://api.scryfall.com/cards";

        // Act
        Url actual = new(urlString);

        // Assert
        _ = actual.Should().NotBeNull();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void ImplicitConversion_ToUri_ReturnsCorrectUri()
    {
        // Arrange
        const string urlString = "https://api.scryfall.com/sets/test";
        Url url = new(urlString);

        // Act
        Uri actual = url;

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.AbsoluteUri.Should().Be(urlString);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Constructor_InvalidUrl_ThrowsUriFormatException()
    {
        // Arrange
        const string invalidUrl = "not a valid url";

        // Act
        Action act = () => _ = new Url(invalidUrl);

        // Assert
        act.Should().Throw<UriFormatException>();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Constructor_NullUrl_ThrowsArgumentNullException()
    {
        // Arrange
        string nullUrl = null!;

        // Act
        Action act = () => _ = new Url(nullUrl);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Constructor_EmptyUrl_ThrowsUriFormatException()
    {
        // Arrange
        const string emptyUrl = "";

        // Act
        Action act = () => _ = new Url(emptyUrl);

        // Assert
        act.Should().Throw<UriFormatException>();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void ToString_ValidUrl_ReturnsUrlString()
    {
        // Arrange
        const string urlString = "https://api.scryfall.com/sets";
        Url url = new(urlString);

        // Act
        string actual = url.ToString();

        // Assert
        _ = actual.Should().Be(urlString);
    }
}
