using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.Aggregator.SealedProducts.Queries.Mappers;

internal sealed class CollectionSealedProductExtToOufMapper
    : CollectionCreateMapper<SealedProductExtEntity, ISealedProductOufEntity>,
      ICollectionSealedProductExtToOufMapper
{
    public CollectionSealedProductExtToOufMapper() : this(new SealedProductExtToOufMapper()) { }

    private CollectionSealedProductExtToOufMapper(ISealedProductExtToOufMapper mapper) : base(mapper) { }
}
