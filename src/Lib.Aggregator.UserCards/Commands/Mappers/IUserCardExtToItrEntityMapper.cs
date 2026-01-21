using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal interface IUserCardExtToItrEntityMapper : ICreateMapper<UserCardExtEntity, IUserCardOufEntity>
{
}
