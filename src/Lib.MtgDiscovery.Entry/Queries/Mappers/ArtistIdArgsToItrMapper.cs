using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistIdArgsToItrMapper : IArtistIdArgsToItrMapper
{
    public async Task<IArtistIdItrEntity> Map(IArtistIdArgEntity args)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        return new ArtistIdItrEntity { ArtistId = args.ArtistId };
    }
}