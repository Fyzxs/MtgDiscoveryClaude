using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Artists.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Queries;

internal sealed class ArtistsQueryDomainService : IArtistsQueryDomainService
{
    private readonly IArtistSearchDomain _artistSearchService;
    private readonly ICardsByArtistDomain _cardsByArtistService;
    private readonly ICardsByArtistNameDomain _cardsByArtistNameService;

    public ArtistsQueryDomainService(ILogger logger) : this(
        new ArtistSearchDomain(logger),
        new CardsByArtistDomain(logger),
        new CardsByArtistNameDomain(logger))
    { }

    private ArtistsQueryDomainService(
        IArtistSearchDomain artistSearchService,
        ICardsByArtistDomain cardsByArtistService,
        ICardsByArtistNameDomain cardsByArtistNameService)
    {
        _artistSearchService = artistSearchService;
        _cardsByArtistService = cardsByArtistService;
        _cardsByArtistNameService = cardsByArtistNameService;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(
        IArtistSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken)
        => await _artistSearchService.Execute(searchTerm, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(
        IArtistIdItrEntity artistId,
        CancellationToken cancellationToken)
        => await _cardsByArtistService.Execute(artistId, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(
        IArtistNameItrEntity artistName,
        CancellationToken cancellationToken)
        => await _cardsByArtistNameService.Execute(artistName, cancellationToken).ConfigureAwait(false);
}
