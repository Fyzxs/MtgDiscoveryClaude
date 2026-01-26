using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Resolvers;

namespace Lib.Adapter.UserCards.Commands.Resolvers;

/// <summary>
/// Resolves a UserCardExtEntity from a Cosmos read response, creating a new entity if none exists.
/// </summary>
internal interface IUserCardResolver : IResolver<OpResponse<UserCardExtEntity>, UserCardExtEntity, IAddUserCardXfrEntity>
{
}
