using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Cosmos.Resolvers;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Resolvers;

internal interface IAddSetGroupResolver : ICosmosResolver<UserSetCardExtEntity, IAddSetGroupToUserSetCardXfrEntity>;
