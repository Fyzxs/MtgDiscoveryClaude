using System.Collections.Generic;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal interface ICollectionSetCodeItrToReadPointItemMapper : ICreateMapper<ISetCodesItrEntity, IEnumerable<ReadPointItem>>;
