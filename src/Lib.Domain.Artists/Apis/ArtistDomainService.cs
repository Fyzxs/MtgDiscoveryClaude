using System.Threading.Tasks;
using Lib.Domain.Artists.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Artists.Apis;

public sealed class ArtistDomainService : IArtistDomainService
{
    private readonly IArtistDomainService _artistDomainOperations;

    public ArtistDomainService(ILogger logger) : this(new QueryArtistDomainService(logger))
    { }

    private ArtistDomainService(IArtistDomainService artistDomainOperations) => _artistDomainOperations = artistDomainOperations;

    public Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm) => _artistDomainOperations.ArtistSearchAsync(searchTerm);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId) => _artistDomainOperations.CardsByArtistAsync(artistId);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName) => _artistDomainOperations.CardsByArtistNameAsync(artistName);
}
