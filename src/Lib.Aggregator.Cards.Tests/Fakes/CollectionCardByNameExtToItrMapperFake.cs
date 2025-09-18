using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Aggregator.Cards.Queries.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CollectionCardByNameExtToItrMapperFake : ICollectionCardByNameExtToItrMapper
{
    public IEnumerable<ICardItemItrEntity> MapResult { get; init; } = new List<ICardItemItrEntity>();
    public int MapInvokeCount { get; private set; }
    public IEnumerable<ScryfallCardByNameExtEntity> MapSourceInput { get; private set; } = default!;

    public Task<IEnumerable<ICardItemItrEntity>> Map(IEnumerable<ScryfallCardByNameExtEntity> source)
    {
        MapInvokeCount++;
        MapSourceInput = source;
        return Task.FromResult(MapResult);
    }
}
