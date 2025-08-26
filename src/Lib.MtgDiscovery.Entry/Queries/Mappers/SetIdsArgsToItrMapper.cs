using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class SetIdsArgsToItrMapper : ISetIdsArgsToItrMapper
{
    public Task<ISetIdsItrEntity> Map(ISetIdsArgEntity arg)
    {
        List<string> setIds = [.. arg.SetIds];
        return Task.FromResult<ISetIdsItrEntity>(new EntrySetIdsItrEntity(setIds));
    }
}