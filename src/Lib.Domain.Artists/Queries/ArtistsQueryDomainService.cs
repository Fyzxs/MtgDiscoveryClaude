using System.Threading.Tasks;
using Lib.Aggregator.Artists.Apis;
using Lib.Domain.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Queries;

internal sealed class ArtistsQueryDomainService : IArtistsQueryDomainService
{
    private readonly IArtistSearchDomainService _artistSearchService;
    private readonly ICardsByArtistDomainService _cardsByArtistService;
    private readonly ICardsByArtistNameDomainService _cardsByArtistNameService;

    public ArtistsQueryDomainService(ILogger logger) : this(
        new ArtistSearchDomainService(logger),
        new CardsByArtistDomainService(logger),
        new CardsByArtistNameDomainService(logger))
    { }

    private ArtistsQueryDomainService(
        IArtistSearchDomainService artistSearchService,
        ICardsByArtistDomainService cardsByArtistService,
        ICardsByArtistNameDomainService cardsByArtistNameService)
    {
        _artistSearchService = artistSearchService;
        _cardsByArtistService = cardsByArtistService;
        _cardsByArtistNameService = cardsByArtistNameService;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
        => await _artistSearchService.Execute(searchTerm).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
        => await _cardsByArtistService.Execute(artistId).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
        => await _cardsByArtistNameService.Execute(artistName).ConfigureAwait(false);
}
