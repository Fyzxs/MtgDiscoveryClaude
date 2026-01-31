using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public static class ScryfallCardItemFactoryFake
{
    public static ScryfallCardItemExtEntity Create(dynamic data) => new() { Data = data };
}
