using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Artists.Apis;

public interface IArtistsQueryAggregatorService
{
    Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm);
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId);
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName);
}
