using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal static class FakeScryfallCardItemFactory
{
    public static ScryfallCardExtArg Create(dynamic data)
    {
        return new ScryfallCardExtArg { Data = data };
    }
}
