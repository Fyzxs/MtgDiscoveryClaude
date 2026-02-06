using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAssertions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Scryfall.Ingestion.Mappers;
using Lib.Scryfall.Ingestion.Services;
using Lib.Scryfall.Ingestion.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TestConvenience.Core.Reflection;

namespace Lib.Scryfall.Ingestion.Tests.Mappers;

[TestClass]
public sealed class ScryfallSetToCosmosMapperTests
{
    [TestMethod, TestCategory("unit")]
    public async Task Map_SetWithoutGroupings_ReturnsEntityWithOriginalData()
    {
        // Arrange
        SetGroupingsLoaderFake loader = new();
        ScryfallSetToCosmosMapper subject = new InstanceWrapper(loader);
        dynamic data = new { id = "set-1", name = "Test Set" };
        ScryfallSetFake set = new(code: "tst", data: (object)data);

        // Act
        ScryfallSetItemExtEntity actual = await subject.Map(set).ConfigureAwait(false);

        // Assert
        string id = actual.Data.id;
        id.Should().Be("set-1");
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_SetWithGroupings_AddsGroupingsToData()
    {
        // Arrange
        SetGroupingsLoaderFake loader = new();
        loader.AddGrouping("tst", new SetGroupingData
        {
            SetCode = "tst",
            Groupings =
            [
                new CardGrouping { Id = "grp-1", DisplayName = "Main Set", Order = 1 }
            ]
        });
        ScryfallSetToCosmosMapper subject = new InstanceWrapper(loader);
        dynamic data = new { id = "set-1", name = "Test Set" };
        ScryfallSetFake set = new(code: "tst", data: (object)data);

        // Act
        ScryfallSetItemExtEntity actual = await subject.Map(set).ConfigureAwait(false);

        // Assert
        JObject resultData = (JObject)actual.Data;
        resultData["groupings"].Should().NotBeNull();
    }

    [TestMethod, TestCategory("unit")]
    public async Task Map_SetWithGroupings_PreservesOriginalData()
    {
        // Arrange
        SetGroupingsLoaderFake loader = new();
        loader.AddGrouping("tst", new SetGroupingData
        {
            SetCode = "tst",
            Groupings =
            [
                new CardGrouping { Id = "grp-1", DisplayName = "Main Set", Order = 1 }
            ]
        });
        ScryfallSetToCosmosMapper subject = new InstanceWrapper(loader);
        dynamic data = new { id = "set-1", name = "Test Set" };
        ScryfallSetFake set = new(code: "tst", data: (object)data);

        // Act
        ScryfallSetItemExtEntity actual = await subject.Map(set).ConfigureAwait(false);

        // Assert
        JObject resultData = (JObject)actual.Data;
        resultData["id"]!.Value<string>().Should().Be("set-1");
        resultData["name"]!.Value<string>().Should().Be("Test Set");
    }

    private sealed class InstanceWrapper : TypeWrapper<ScryfallSetToCosmosMapper>
    {
        public InstanceWrapper(ISetGroupingsLoader groupingsLoader) : base(groupingsLoader) { }
    }
}
