using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal static class FakeScryfallCardItemFactory
{
    public static ScryfallCardItem Create(dynamic data)
    {
        return new ScryfallCardItem { Data = data };
    }
}
