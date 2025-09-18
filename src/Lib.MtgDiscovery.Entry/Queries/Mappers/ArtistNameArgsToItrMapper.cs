using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistNameArgsToItrMapper : IArtistNameArgsToItrMapper
{
    public Task<IArtistNameItrEntity> Map(IArtistNameArgEntity args)
    {
        return Task.FromResult<IArtistNameItrEntity>(new ArtistNameItrEntity { ArtistName = args.ArtistName });
    }
}
