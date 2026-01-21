using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.DataModels.Entities.Xfrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Single-method adapter for retrieving all sets.
/// </summary>
internal interface IAllSetsAdapter
    : IOperationResponseService<IAllSetsXfrEntity, IEnumerable<ScryfallSetItemExtEntity>>;
