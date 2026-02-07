using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace Lib.Adapter.Collections.Commands.Mappers;

internal interface ICollectionXfrToExtMapper
    : ICreateMapper<ICollectionXfrEntity, CollectionExtEntity>;
