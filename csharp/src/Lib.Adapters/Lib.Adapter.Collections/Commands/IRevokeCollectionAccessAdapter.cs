using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Collections.Commands;

internal interface IRevokeCollectionAccessAdapter
    : IOperationResponseService<IRevokeCollectionAccessXfrEntity, CollectionExtEntity>;
