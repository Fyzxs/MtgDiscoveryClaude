using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class SetCodeArgsToItrMapper : ISetCodeArgsToItrMapper
{
    public Task<ISetCodeItrEntity> Map(ISetCodeArgEntity args)
    {
        return Task.FromResult<ISetCodeItrEntity>(new SetCodeItrEntity
        {
            SetCode = args.SetCode
        });
    }
}
