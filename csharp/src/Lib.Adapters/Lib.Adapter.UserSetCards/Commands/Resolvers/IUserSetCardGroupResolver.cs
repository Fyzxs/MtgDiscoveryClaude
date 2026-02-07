using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Resolvers;

namespace Lib.Adapter.UserSetCards.Commands.Resolvers;

internal interface IUserSetCardGroupResolver : IResolver<UserSetCardExtEntity, Dictionary<string, UserSetCardFinishGroupExtEntity>, IAddCardToSetXfrEntity>;
