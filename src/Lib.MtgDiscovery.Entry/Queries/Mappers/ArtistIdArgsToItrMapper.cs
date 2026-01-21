using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistIdArgsToItrMapper : IArtistIdArgsToItrMapper
{
    public Task<IArtistIdItrEntity> Map(IArtistIdArgEntity args)
    {
        return Task.FromResult<IArtistIdItrEntity>(new ArtistIdItrEntity { ArtistId = args.ArtistId });
    }
}
