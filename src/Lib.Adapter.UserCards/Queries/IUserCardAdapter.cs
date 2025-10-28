using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Adapter for retrieving a specific user card using point read.
/// </summary>
internal interface IUserCardAdapter
    : IOperationResponseService<IUserCardXfrEntity, IEnumerable<UserCardExtEntity>>;
