using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Sets.Apis.Entities;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Single-method adapter for retrieving sets by their IDs.
/// </summary>
internal interface ISetsByIdsAdapter
    : IOperationResponseService<ISetIdsXfrEntity, IEnumerable<ScryfallSetItemExtEntity>>;
