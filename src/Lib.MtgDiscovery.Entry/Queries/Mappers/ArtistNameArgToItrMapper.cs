using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistNameArgToItrMapper : IArtistNameArgToItrMapper
{
    public Task<IArtistNameItrEntity> Map(IArtistNameArgEntity args) => Task.FromResult<IArtistNameItrEntity>(new ArtistNameItrEntity { ArtistName = args.ArtistName });
}
