using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistNameArgsToItrMapper : IArtistNameArgsToItrMapper
{
    public async Task<IArtistNameItrEntity> Map(IArtistNameArgEntity args)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        return new ArtistNameItrEntity { ArtistName = args.ArtistName };
    }
}