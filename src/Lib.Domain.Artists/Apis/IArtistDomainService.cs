using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Artists.Apis;

public interface IArtistDomainService
{
    Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId);
}