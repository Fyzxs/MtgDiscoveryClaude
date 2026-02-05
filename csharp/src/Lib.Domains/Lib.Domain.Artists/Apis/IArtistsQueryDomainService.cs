using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Apis;

public interface IArtistsQueryDomainService
{
    Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(
        IArtistSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken);

    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(
        IArtistIdItrEntity artistId,
        CancellationToken cancellationToken);

    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(
        IArtistNameItrEntity artistName,
        CancellationToken cancellationToken);
}
