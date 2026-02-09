using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Aggregator.Collections.Mappers;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Aggregator.Collections.Queries.Mappers;

internal sealed class CollectionCollectionExtToOufMapper
    : CollectionCreateMapper<CollectionExtEntity, ICollectionOufEntity>,
      ICollectionCollectionExtToOufMapper
{
    public CollectionCollectionExtToOufMapper() : this(new CollectionExtToOufMapper()) { }

    private CollectionCollectionExtToOufMapper(ICollectionExtToOufMapper mapper) : base(mapper) { }
}
