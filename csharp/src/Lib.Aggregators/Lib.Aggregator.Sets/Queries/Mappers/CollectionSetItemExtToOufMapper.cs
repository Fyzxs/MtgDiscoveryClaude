using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal sealed class CollectionSetItemExtToOufMapper : CollectionCreateMapper<ScryfallSetItemExtEntity, ISetItemOufEntity>, ICollectionSetItemExtToOufMapper
{
    public CollectionSetItemExtToOufMapper() : base(new SetItemExtToOufMapper())
    { }
}
