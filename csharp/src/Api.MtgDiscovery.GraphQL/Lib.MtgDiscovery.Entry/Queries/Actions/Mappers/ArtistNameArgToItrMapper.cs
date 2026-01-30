using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class ArtistNameArgToItrMapper : IArtistNameArgToItrMapper
{
    public Task<IArtistNameItrEntity> Map(IArtistNameArgEntity args) => Task.FromResult<IArtistNameItrEntity>(new ArtistNameItrEntity { ArtistName = args.ArtistName });
}
