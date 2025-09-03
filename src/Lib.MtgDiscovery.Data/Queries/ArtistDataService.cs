using System.Threading.Tasks;
using Lib.Domain.Artists.Apis;
using Lib.MtgDiscovery.Data.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Data.Queries;

internal sealed class ArtistDataService : IArtistDataService
{
    private readonly IArtistDomainService _artistDomainService;

    public ArtistDataService(ILogger logger) : this(new ArtistDomainService(logger))
    {

    }
    public ArtistDataService(IArtistDomainService artistDomainService)
    {
        _artistDomainService = artistDomainService;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        return await _artistDomainService.ArtistSearchAsync(searchTerm).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        return await _artistDomainService.CardsByArtistAsync(artistId).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        return await _artistDomainService.CardsByArtistNameAsync(artistName).ConfigureAwait(false);
    }
}