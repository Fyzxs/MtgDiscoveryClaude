using AwesomeAssertions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetParentAssociations;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Scryfall.Ingestion.Tests.Mappers;

[TestClass]
public sealed class SetToSetParentAssociationExtMapperTests
{
    [TestMethod, TestCategory("unit")]
    public void HasParentSet_SetWithParent_ReturnsTrue()
    {
        // Arrange
        SetToSetParentAssociationExtMapper subject = new();
        ScryfallSetFake set = new(parentSetCode: "parent-code");

        // Act
        bool actual = subject.HasParentSet(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void HasParentSet_SetWithoutParent_ReturnsFalse()
    {
        // Arrange
        SetToSetParentAssociationExtMapper subject = new();
        ScryfallSetFake set = new();

        // Act
        bool actual = subject.HasParentSet(set);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void HasNoParentSet_SetWithoutParent_ReturnsTrue()
    {
        // Arrange
        SetToSetParentAssociationExtMapper subject = new();
        ScryfallSetFake set = new();

        // Act
        bool actual = subject.HasNoParentSet(set);

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void HasNoParentSet_SetWithParent_ReturnsFalse()
    {
        // Arrange
        SetToSetParentAssociationExtMapper subject = new();
        ScryfallSetFake set = new(parentSetCode: "parent-code");

        // Act
        bool actual = subject.HasNoParentSet(set);

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void Map_SetWithParent_ReturnsCorrectEntity()
    {
        // Arrange
        SetToSetParentAssociationExtMapper subject = new();
        ScryfallSetFake set = new(
            code: "tst",
            name: "Test Set",
            id: "set-123",
            parentSetCode: "par");

        // Act
        ScryfallSetParentAssociationExtEntity actual = subject.Map(set);

        // Assert
        actual.SetId.Should().Be("set-123");
        actual.ParentSetCode.Should().Be("par");
        actual.SetCode.Should().Be("tst");
        actual.SetName.Should().Be("Test Set");
    }

    [TestMethod, TestCategory("unit")]
    public void Map_SetWithoutParent_ReturnsEntityWithEmptyParentCode()
    {
        // Arrange
        SetToSetParentAssociationExtMapper subject = new();
        ScryfallSetFake set = new(code: "tst", name: "Test Set", id: "set-123");

        // Act
        ScryfallSetParentAssociationExtEntity actual = subject.Map(set);

        // Assert
        actual.SetId.Should().Be("set-123");
        actual.ParentSetCode.Should().BeEmpty();
        actual.SetCode.Should().Be("tst");
        actual.SetName.Should().Be("Test Set");
    }
}
