using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class SetIdsArgToItrMapper : ISetIdsArgToItrMapper
{
    public Task<ISetIdsItrEntity> Map(ISetIdsArgEntity arg) => Task.FromResult<ISetIdsItrEntity>(new EntrySetIdsItrEntity(arg.SetIds));
}
