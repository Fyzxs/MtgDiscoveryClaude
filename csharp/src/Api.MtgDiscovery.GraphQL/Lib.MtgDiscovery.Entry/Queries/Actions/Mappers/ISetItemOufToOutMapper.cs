using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISetItemOufToOutMapper : ICreateMapper<ISetItemOufEntity, SetItemOutEntity>
{
}
