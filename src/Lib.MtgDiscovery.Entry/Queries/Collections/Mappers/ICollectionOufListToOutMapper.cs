using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.MtgDiscovery.Entry.Queries.Collections.Mappers;

internal interface ICollectionOufListToOutMapper : ICreateMapper<IEnumerable<ICollectionOufEntity>, List<CollectionOutEntity>>
{
}
