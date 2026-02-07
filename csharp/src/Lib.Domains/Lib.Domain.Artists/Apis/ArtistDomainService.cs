using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Artists.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Apis;

public sealed class ArtistDomainService : IArtistDomainService
{
    private readonly IArtistsQueryDomainService _artistDomainOperations;

    public ArtistDomainService(ILogger logger) : this(new ArtistsQueryDomainService(logger))
    { }

    private ArtistDomainService(IArtistsQueryDomainService artistDomainOperations) => _artistDomainOperations = artistDomainOperations;

    public async Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(
        IArtistSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken) => await _artistDomainOperations.ArtistSearchAsync(searchTerm, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(
        IArtistIdItrEntity artistId,
        CancellationToken cancellationToken) => await _artistDomainOperations.CardsByArtistAsync(artistId, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(
        IArtistNameItrEntity artistName,
        CancellationToken cancellationToken) => await _artistDomainOperations.CardsByArtistNameAsync(artistName, cancellationToken).ConfigureAwait(false);
}
