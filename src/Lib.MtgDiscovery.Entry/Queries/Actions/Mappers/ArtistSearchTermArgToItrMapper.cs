using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class ArtistSearchTermArgToItrMapper : IArtistSearchTermArgToItrMapper
{
    public Task<IArtistSearchTermItrEntity> Map(IArtistSearchTermArgEntity args) => Task.FromResult<IArtistSearchTermItrEntity>(new ArtistSearchTermItrEntity { SearchTerm = args.SearchTerm });
}
