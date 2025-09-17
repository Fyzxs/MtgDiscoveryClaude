using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal static class FakeScryfallCardItemFactory
{
    public static ScryfallCardItemExtEntity Create(dynamic data)
    {
        return new ScryfallCardItemExtEntity { Data = data };
    }
}
