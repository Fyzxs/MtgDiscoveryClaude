using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class SetIdsArgsToItrMapper : ISetIdsArgsToItrMapper
{
    public Task<ISetIdsItrEntity> Map(ISetIdsArgEntity arg)
    {
        return Task.FromResult<ISetIdsItrEntity>(new EntrySetIdsItrEntity(arg.SetIds));
    }
}
