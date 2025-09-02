using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistSearchTermArgsToItrMapper : IArtistSearchTermArgsToItrMapper
{
    public async Task<IArtistSearchTermItrEntity> Map(IArtistSearchTermArgEntity args)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        return new ArtistSearchTermItrEntity { SearchTerm = args.SearchTerm };
    }
}