using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISetItemOufToOutMapper : ICreateMapper<ISetItemItrEntity, SetItemOutEntity>
{
}
