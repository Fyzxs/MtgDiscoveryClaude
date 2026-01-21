using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Resolvers;

namespace Lib.Adapter.UserSetCards.Commands.Resolvers;

internal interface IUserSetCardGroupResolver : IResolver<UserSetCardExtEntity, Dictionary<string, UserSetCardFinishGroupExtEntity>, IAddCardToSetXfrEntity>;
