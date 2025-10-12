using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IArtistEntryService
{
    Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm);
    Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(IArtistIdArgEntity artistId);
    Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistNameAsync(IArtistNameArgEntity artistName);
}
