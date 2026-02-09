using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Queries.Mappers;

internal sealed class CollectionUserSealedProductExtToOufMapper
    : CollectionCreateMapper<UserSealedProductExtEntity, IUserSealedProductOufEntity>,
      ICollectionUserSealedProductExtToOufMapper
{
    public CollectionUserSealedProductExtToOufMapper() : this(new UserSealedProductExtToOufMapper()) { }

    private CollectionUserSealedProductExtToOufMapper(IUserSealedProductExtToOufMapper mapper) : base(mapper) { }
}
