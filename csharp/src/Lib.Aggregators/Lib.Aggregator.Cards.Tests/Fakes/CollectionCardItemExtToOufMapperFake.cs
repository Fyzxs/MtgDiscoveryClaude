using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CollectionCardItemExtToOufMapperFake : ICollectionCardItemExtToOufMapper
{
    public IEnumerable<ICardItemOufEntity> MapResult { get; init; } = [];
    public int MapInvokeCount { get; private set; }
    public IEnumerable<ScryfallCardItemExtEntity> MapSourceInput { get; private set; } = default!;

    public Task<IEnumerable<ICardItemOufEntity>> Map(IEnumerable<ScryfallCardItemExtEntity> source)
    {
        MapInvokeCount++;
        MapSourceInput = source;
        return Task.FromResult(MapResult);
    }
}
