using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class SetCodesArgToItrMapper : ISetCodesArgToItrMapper
{
    public Task<ISetCodesItrEntity> Map(ISetCodesArgEntity arg)
    {
        List<string> setCodes = [.. arg.SetCodes];
        return Task.FromResult<ISetCodesItrEntity>(new EntrySetCodesItrEntity(setCodes));
    }
}
