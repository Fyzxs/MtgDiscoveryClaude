using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Aggregator.Collections.Queries.Mappers;

internal interface ICollectionCollectionExtToOufMapper : ICreateMapper<IEnumerable<CollectionExtEntity>, IEnumerable<ICollectionOufEntity>>;
