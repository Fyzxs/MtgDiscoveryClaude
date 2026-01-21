using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Tests.CosmosItems;

[TestClass]
public sealed class UserCardItemTests
{
    [TestMethod, TestCategory("unit")]
    public void Constructor_WithInitializers_CreatesInstance()
    {
        // Arrange & Act
        UserCardItem actual = new()
        {
            UserId = "test-user-id",
            CardId = "test-card-id",
            SetId = "test-set-id",
            CollectedList = []
        };

        // Assert
        actual.Should().NotBeNull();
        actual.UserId.Should().Be("test-user-id");
        actual.CardId.Should().Be("test-card-id");
        actual.SetId.Should().Be("test-set-id");
        actual.CollectedList.Should().NotBeNull();
        actual.CollectedList.Should().BeEmpty();
    }

    [TestMethod, TestCategory("unit")]
    public void Id_ReturnsCardIdValue()
    {
        // Arrange
        UserCardItem actual = new()
        {
            CardId = "expected-card-id"
        };

        // Act & Assert
        actual.Id.Should().Be("expected-card-id");
    }

    [TestMethod, TestCategory("unit")]
    public void Partition_ReturnsUserIdValue()
    {
        // Arrange
        UserCardItem actual = new()
        {
            UserId = "expected-user-id"
        };

        // Act & Assert
        actual.Partition.Should().Be("expected-user-id");
    }

    [TestMethod, TestCategory("unit")]
    public void JsonPropertyAttributes_AreConfiguredCorrectly()
    {
        // Arrange
        PropertyInfo userIdProperty = typeof(UserCardItem).GetProperty(nameof(UserCardItem.UserId));
        PropertyInfo cardIdProperty = typeof(UserCardItem).GetProperty(nameof(UserCardItem.CardId));
        PropertyInfo setIdProperty = typeof(UserCardItem).GetProperty(nameof(UserCardItem.SetId));
        PropertyInfo collectedListProperty = typeof(UserCardItem).GetProperty(nameof(UserCardItem.CollectedList));

        // Act
        JsonPropertyAttribute userIdJsonAttribute = userIdProperty?.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() as JsonPropertyAttribute;
        JsonPropertyAttribute cardIdJsonAttribute = cardIdProperty?.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() as JsonPropertyAttribute;
        JsonPropertyAttribute setIdJsonAttribute = setIdProperty?.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() as JsonPropertyAttribute;
        JsonPropertyAttribute collectedListJsonAttribute = collectedListProperty?.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() as JsonPropertyAttribute;

        // Assert
        userIdJsonAttribute.Should().NotBeNull();
        userIdJsonAttribute!.PropertyName.Should().Be("user_id");

        cardIdJsonAttribute.Should().NotBeNull();
        cardIdJsonAttribute!.PropertyName.Should().Be("card_id");

        setIdJsonAttribute.Should().NotBeNull();
        setIdJsonAttribute!.PropertyName.Should().Be("set_id");

        collectedListJsonAttribute.Should().NotBeNull();
        collectedListJsonAttribute!.PropertyName.Should().Be("collected");
    }

    [TestMethod, TestCategory("unit")]
    public void CollectedItem_ConstructorWithInitializers_CreatesInstance()
    {
        // Arrange & Act
        CollectedItem actual = new()
        {
            Finish = "foil",
            Special = "signed",
            Count = 2
        };

        // Assert
        actual.Should().NotBeNull();
        actual.Finish.Should().Be("foil");
        actual.Special.Should().Be("signed");
        actual.Count.Should().Be(2);
    }

    [TestMethod, TestCategory("unit")]
    public void CollectedItem_JsonPropertyAttributes_AreConfiguredCorrectly()
    {
        // Arrange
        PropertyInfo finishProperty = typeof(CollectedItem).GetProperty(nameof(CollectedItem.Finish));
        PropertyInfo specialProperty = typeof(CollectedItem).GetProperty(nameof(CollectedItem.Special));
        PropertyInfo countProperty = typeof(CollectedItem).GetProperty(nameof(CollectedItem.Count));

        // Act
        JsonPropertyAttribute finishJsonAttribute = finishProperty?.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() as JsonPropertyAttribute;
        JsonPropertyAttribute specialJsonAttribute = specialProperty?.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() as JsonPropertyAttribute;
        JsonPropertyAttribute countJsonAttribute = countProperty?.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault() as JsonPropertyAttribute;

        // Assert
        finishJsonAttribute.Should().NotBeNull();
        finishJsonAttribute!.PropertyName.Should().Be("finish");

        specialJsonAttribute.Should().NotBeNull();
        specialJsonAttribute!.PropertyName.Should().Be("special");

        countJsonAttribute.Should().NotBeNull();
        countJsonAttribute!.PropertyName.Should().Be("count");
    }
}
