using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetCards;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CollectionSetCardItemExtToOufMapperFake : ICollectionSetCardItemExtToOufMapper
{
    public IEnumerable<ICardItemItrEntity> MapResult { get; init; } = [];
    public int MapInvokeCount { get; private set; }
    public IEnumerable<ScryfallSetCardItemExtEntity> MapSourceInput { get; private set; } = default!;

    public Task<IEnumerable<ICardItemItrEntity>> Map(IEnumerable<ScryfallSetCardItemExtEntity> source)
    {
        MapInvokeCount++;
        MapSourceInput = source;
        return Task.FromResult(MapResult);
    }
}
