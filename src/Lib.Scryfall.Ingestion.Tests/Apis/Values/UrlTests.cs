using System;
using Lib.Universal.Primitives;

namespace Lib.Scryfall.Ingestion.Tests.Apis.Values;

[TestClass]
public sealed class UrlTests
{
    [TestMethod]
    [TestCategory("unit")]
    public void AsSystemType_ValidUrl_ReturnsUri()
    {
        // Arrange
        const string UrlString = "https://api.scryfall.com/sets";
        Url url = new ProvidedUrl(UrlString);

        // Act
        Uri actual = url.AsSystemType();

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.AbsoluteUri.Should().Be(UrlString);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Constructor_ValidUrl_CreatesInstance()
    {
        // Arrange
        const string UrlString = "https://api.scryfall.com/cards";

        // Act
        Url actual = new ProvidedUrl(UrlString);

        // Assert
        _ = actual.Should().NotBeNull();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void ImplicitConversion_ToUri_ReturnsCorrectUri()
    {
        // Arrange
        const string UrlString = "https://api.scryfall.com/sets/test";
        Url url = new ProvidedUrl(UrlString);

        // Act
        Uri actual = url;

        // Assert
        _ = actual.Should().NotBeNull();
        _ = actual.AbsoluteUri.Should().Be(UrlString);
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Constructor_InvalidUrl_ThrowsUriFormatException()
    {
        // Arrange
        const string InvalidUrl = "not a valid url";

        // Act
        Action act = () => _ = new ProvidedUrl(InvalidUrl);

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
        Action act = () => _ = new ProvidedUrl(nullUrl);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void Constructor_EmptyUrl_ThrowsUriFormatException()
    {
        // Arrange
        const string EmptyUrl = "";

        // Act
        Action act = () => _ = new ProvidedUrl(EmptyUrl);

        // Assert
        act.Should().Throw<UriFormatException>();
    }

    [TestMethod]
    [TestCategory("unit")]
    public void ToString_ValidUrl_ReturnsUrlString()
    {
        // Arrange
        const string UrlString = "https://api.scryfall.com/sets";
        Url url = new ProvidedUrl(UrlString);

        // Act
        string actual = url.ToString();

        // Assert
        _ = actual.Should().Be(UrlString);
    }
}
