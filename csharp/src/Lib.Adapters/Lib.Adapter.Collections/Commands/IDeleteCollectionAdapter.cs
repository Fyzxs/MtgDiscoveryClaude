using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.Collections.Commands;

internal interface IDeleteCollectionAdapter
    : IOperationResponseService<IDeleteCollectionXfrEntity, CollectionExtEntity>;
