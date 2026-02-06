using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Abstractions.Actions.Integrators;

namespace Lib.Adapter.Collections.Commands.Integrators;

internal interface IRenameCollectionIntegrator
    : IIntegrator<CollectionExtEntity, IRenameCollectionXfrEntity>;
