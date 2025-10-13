using System.Collections.Generic;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal interface ICollectionSetItemItrToOufMapper : ICreateMapper<IEnumerable<ISetItemItrEntity>, ISetItemCollectionOufEntity>
{
}
