using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSetCards.Queries;

internal interface IAllUserSetCardsAdapter
    : IOperationResponseService<IAllUserSetCardsXfrEntity, IEnumerable<UserSetCardExtEntity>>;
