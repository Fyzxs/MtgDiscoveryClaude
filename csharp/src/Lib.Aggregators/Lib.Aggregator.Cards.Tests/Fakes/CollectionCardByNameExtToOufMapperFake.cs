using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CollectionCardByNameExtToOufMapperFake : ICollectionCardByNameExtToOufMapper
{
    public IEnumerable<ICardItemItrEntity> MapResult { get; init; } = [];
    public int MapInvokeCount { get; private set; }
    public IEnumerable<ScryfallCardByNameExtEntity> MapSourceInput { get; private set; } = default!;

    public Task<IEnumerable<ICardItemItrEntity>> Map(IEnumerable<ScryfallCardByNameExtEntity> source)
    {
        MapInvokeCount++;
        MapSourceInput = source;
        return Task.FromResult(MapResult);
    }
}
