using AwesomeAssertions;
using Cli.MtgDiscovery.DataMigration.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cli.MtgDiscovery.DataMigration.Tests.Collections;

[TestClass]
public sealed class CollectionMigrationResultTests
{
    [TestMethod]
    public void CollectionMigrationResult_ShouldStoreAllProperties()
    {
        // Arrange & Act
        CollectionMigrationResult actual = new()
        {
            TotalUsers = 10,
            CollectionsCreated = 5,
            CollectionsSkipped = 5,
            UserCardsUpdated = 100,
            UserWishlistCardsUpdated = 50,
            UserSetCardsUpdated = 25,
            Errors = 2
        };

        // Assert
        actual.TotalUsers.Should().Be(10);
        actual.CollectionsCreated.Should().Be(5);
        actual.CollectionsSkipped.Should().Be(5);
        actual.UserCardsUpdated.Should().Be(100);
        actual.UserWishlistCardsUpdated.Should().Be(50);
        actual.UserSetCardsUpdated.Should().Be(25);
        actual.Errors.Should().Be(2);
    }
}
