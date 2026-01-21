using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.DataModels.Entities.Itrs.Sets;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class SetCodeArgToItrMapper : ISetCodeArgToItrMapper
{
    public Task<ISetCodeItrEntity> Map(ISetCodeArgEntity args)
    {
        return Task.FromResult<ISetCodeItrEntity>(new SetCodeItrEntity
        {
            SetCode = args.SetCode
        });
    }
}
