using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving all user cards within a specific set.
/// </summary>
internal interface IUserCardsBySetAdapter
    : IOperationResponseService<IUserCardsSetXfrEntity, IEnumerable<UserCardExtEntity>>;
