using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;

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
