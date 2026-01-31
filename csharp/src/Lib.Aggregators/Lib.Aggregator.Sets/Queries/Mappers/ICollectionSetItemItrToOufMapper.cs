using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.Aggregator.Sets.Queries.Mappers;

internal interface ICollectionSetItemItrToOufMapper : ICreateMapper<IEnumerable<ISetItemItrEntity>, ISetItemCollectionOufEntity>
{
}
