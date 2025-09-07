using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistSearchTermArgsToItrMapper : IArtistSearchTermArgsToItrMapper
{
    public Task<IArtistSearchTermItrEntity> Map(IArtistSearchTermArgEntity args)
    {
        return Task.FromResult<IArtistSearchTermItrEntity>(new ArtistSearchTermItrEntity { SearchTerm = args.SearchTerm });
    }
}
