using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.MtgDiscovery.Entry.Tests.Queries.Mappers;

[TestClass]
public sealed class CardsArgsToItrMapperTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Map_WithValidArgs_ReturnsEntryCardIdsItrEntity()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = ["id1", "id2", "id3"] };
        CardsArgsToItrMapper subject = new();

        // Act
        ICardIdsItrEntity actual = await subject.Map(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.CardIds.Should().NotBeNull();
        actual.CardIds.Count.Should().Be(3);
        actual.CardIds.Should().BeEquivalentTo(args.CardIds);
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_WithEmptyCardIds_ReturnsEmptyCollection()
    {
        // Arrange
        FakeCardIdsArgEntity args = new() { CardIds = [] };
        CardsArgsToItrMapper subject = new();

        // Act
        ICardIdsItrEntity actual = await subject.Map(args).ConfigureAwait(false);

        // Assert
        actual.Should().NotBeNull();
        actual.CardIds.Should().NotBeNull();
        actual.CardIds.Should().BeEmpty();
    }

    private sealed class FakeCardIdsArgEntity : ICardIdsArgEntity
    {
        public ICollection<string> CardIds { get; init; } = [];
    }
}
