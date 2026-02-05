using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Cosmos.Resolvers;

namespace Lib.Adapter.UserCards.Commands.Resolvers;

/// <summary>
/// Resolves a UserCardExtEntity from a Cosmos read response, creating a new entity if none exists.
/// </summary>
internal interface IUserCardResolver : ICosmosResolver<UserCardExtEntity, IAddUserCardXfrEntity>;
