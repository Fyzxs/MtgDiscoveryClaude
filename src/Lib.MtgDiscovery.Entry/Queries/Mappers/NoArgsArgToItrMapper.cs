using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class NoArgsArgToItrMapper : INoArgsArgToItrMapper
{
    public Task<INoArgsItrEntity> Map(INoArgsArgEntity source)
    {
        INoArgsItrEntity result = new NoArgsItrEntity();
        return Task.FromResult(result);
    }
}
