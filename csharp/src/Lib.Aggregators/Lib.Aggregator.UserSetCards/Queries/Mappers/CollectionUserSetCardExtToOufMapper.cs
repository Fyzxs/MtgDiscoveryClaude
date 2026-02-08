using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class CollectionUserSetCardExtToOufMapper : CollectionCreateMapper<UserSetCardExtEntity, IUserSetCardOufEntity>, ICollectionUserSetCardExtToOufMapper
{
    public CollectionUserSetCardExtToOufMapper() : base(new UserSetCardExtToOufMapper())
    { }
}
