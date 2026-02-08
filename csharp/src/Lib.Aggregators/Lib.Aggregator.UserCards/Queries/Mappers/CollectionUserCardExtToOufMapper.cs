using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Aggregator.UserCards.Commands.Mappers;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal sealed class CollectionUserCardExtToOufMapper : CollectionCreateMapper<UserCardExtEntity, IUserCardOufEntity>, ICollectionUserCardExtToOufMapper
{
    public CollectionUserCardExtToOufMapper() : base(new UserCardExtToOufEntityMapper())
    { }
}
