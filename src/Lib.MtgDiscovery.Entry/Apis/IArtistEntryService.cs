using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IArtistEntryService
{
    Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdArgEntity artistId);
}