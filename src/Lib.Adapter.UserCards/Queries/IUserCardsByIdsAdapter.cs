using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving multiple user cards using parallel point reads.
/// </summary>
internal interface IUserCardsByIdsAdapter
    : IOperationResponseService<IUserCardsByIdsXfrEntity, IEnumerable<UserCardExtEntity>>;
