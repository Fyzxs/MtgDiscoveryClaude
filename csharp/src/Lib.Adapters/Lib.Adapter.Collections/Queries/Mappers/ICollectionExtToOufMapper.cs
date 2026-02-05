using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal interface ICollectionExtToOufMapper : ICreateMapper<CollectionExtEntity, ICollectionOufEntity>;
