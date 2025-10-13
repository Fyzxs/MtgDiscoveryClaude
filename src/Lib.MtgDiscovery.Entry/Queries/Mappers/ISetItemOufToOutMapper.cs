using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ISetItemOufToOutMapper : ICreateMapper<ISetItemItrEntity, ScryfallSetOutEntity>
{
}
