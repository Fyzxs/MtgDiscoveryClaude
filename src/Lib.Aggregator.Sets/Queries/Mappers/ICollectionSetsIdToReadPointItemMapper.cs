using System.Collections.Generic;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal interface ICollectionSetsIdToReadPointItemMapper : ICreateMapper<ISetIdsItrEntity, IEnumerable<ReadPointItem>>;