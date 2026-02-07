using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.SetItems;
using Lib.Shared.DataModels.Entities.Xfrs.Sets;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Single-method adapter for retrieving all sets.
/// </summary>
internal interface IAllSetsAdapter
    : IOperationResponseService<IAllSetsXfrEntity, IEnumerable<ScryfallSetItemExtEntity>>;
