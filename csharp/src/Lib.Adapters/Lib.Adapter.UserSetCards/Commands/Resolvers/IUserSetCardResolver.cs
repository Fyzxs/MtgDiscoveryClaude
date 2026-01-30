using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Cosmos.Resolvers;

namespace Lib.Adapter.UserSetCards.Commands.Resolvers;

internal interface IUserSetCardResolver : ICosmosResolver<UserSetCardExtEntity, IAddCardToSetXfrEntity>;
