using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class AllSetsArgToItrMapper : IAllSetsArgToItrMapper
{
    public Task<IAllSetsItrEntity> Map(IAllSetsArgEntity source)
    {
        IAllSetsItrEntity result = new AllSetsItrEntity();
        return Task.FromResult(result);
    }
}
