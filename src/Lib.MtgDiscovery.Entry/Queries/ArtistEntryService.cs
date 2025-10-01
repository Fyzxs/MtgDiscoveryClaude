using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Artists;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class ArtistEntryService : IArtistEntryService
{
    private readonly IArtistSearchEntryService _artistSearch;
    private readonly ICardsByArtistEntryService _cardsByArtist;
    private readonly ICardsByArtistNameEntryService _cardsByArtistName;

    public ArtistEntryService(ILogger logger) : this(
        new ArtistSearchEntryService(logger),
        new CardsByArtistEntryService(logger),
        new CardsByArtistNameEntryService(logger))
    { }

    private ArtistEntryService(
        IArtistSearchEntryService artistSearch,
        ICardsByArtistEntryService cardsByArtist,
        ICardsByArtistNameEntryService cardsByArtistName)
    {
        _artistSearch = artistSearch;
        _cardsByArtist = cardsByArtist;
        _cardsByArtistName = cardsByArtistName;
    }

    public async Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm) => await _artistSearch.Execute(searchTerm).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(IArtistIdArgEntity artistId) => await _cardsByArtist.Execute(artistId).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistNameAsync(IArtistNameArgEntity artistName) => await _cardsByArtistName.Execute(artistName).ConfigureAwait(false);
}
