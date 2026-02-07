using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Scryfall.Shared.Entities;
using Lib.Scryfall.Shared.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Shared.Tests.Mappers;

[TestClass]
public sealed class SearchTermToTrigramsMapperTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Map_AllLetterInput_ReturnsCorrectTrigrams()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("Lightning").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("lightning");
        actual.Trigrams.Should().HaveCount(7);
        actual.Trigrams[0].Should().Be("lig");
        actual.Trigrams[1].Should().Be("igh");
        actual.Trigrams[2].Should().Be("ght");
        actual.Trigrams[3].Should().Be("htn");
        actual.Trigrams[4].Should().Be("tni");
        actual.Trigrams[5].Should().Be("nin");
        actual.Trigrams[6].Should().Be("ing");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_MixedCaseInput_NormalizesToLowercase()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("BoLt").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("bolt");
        actual.Trigrams.Should().HaveCount(2);
        actual.Trigrams[0].Should().Be("bol");
        actual.Trigrams[1].Should().Be("olt");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_InputWithNonLetterCharacters_StripsNonLetters()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("A-B C!1D").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("abcd");
        actual.Trigrams.Should().HaveCount(2);
        actual.Trigrams[0].Should().Be("abc");
        actual.Trigrams[1].Should().Be("bcd");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_InputWithSpaces_StripsSpaces()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("Lightning Bolt").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("lightningbolt");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_ExactlyThreeLetters_ReturnsSingleTrigram()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("Abc").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("abc");
        actual.Trigrams.Should().HaveCount(1);
        actual.Trigrams[0].Should().Be("abc");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_TwoLetterInput_ReturnsEmptyTrigrams()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("Ab").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("ab");
        actual.Trigrams.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_SingleLetterInput_ReturnsEmptyTrigrams()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("A").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("a");
        actual.Trigrams.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_EmptyStringInput_ReturnsEmptyNormalizedAndTrigrams()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("");
        actual.Trigrams.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_OnlyNonLetterCharacters_ReturnsEmptyNormalizedAndTrigrams()
    {
        // Arrange
        SearchTermToTrigramsMapper subject = new();

        // Act
        ITrigramCollectionEntity actual = await subject.Map("123!@#").ConfigureAwait(false);

        // Assert
        actual.Normalized.Should().Be("");
        actual.Trigrams.Should().BeEmpty();
    }
}
