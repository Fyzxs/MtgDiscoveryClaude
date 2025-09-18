using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Apis;

public interface IArtistDomainService
{
    Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName);
}
