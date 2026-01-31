using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Mappers;

internal interface ICollectionOufToOutMapper : ICreateMapper<ICollectionOufEntity, CollectionOutEntity>
{
}
