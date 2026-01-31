using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class ArtistIdArgToItrMapper : IArtistIdArgToItrMapper
{
    public Task<IArtistIdItrEntity> Map(IArtistIdArgEntity args) => Task.FromResult<IArtistIdItrEntity>(new ArtistIdItrEntity { ArtistId = args.ArtistId });
}
