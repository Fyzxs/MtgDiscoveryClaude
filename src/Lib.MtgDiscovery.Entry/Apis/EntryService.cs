using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Apis;

public sealed class EntryService : IEntryService
{
    private readonly ICardEntryService _cardEntryService;
    private readonly ISetEntryService _setEntryService;
    private readonly IArtistEntryService _artistEntryService;

    public EntryService(ILogger logger) : this(new CardEntryService(logger), new SetEntryService(logger), new ArtistEntryService(logger))
    {

    }
    private EntryService(ICardEntryService cardEntryService, ISetEntryService setEntryService, IArtistEntryService artistEntryService)
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
        _artistEntryService = artistEntryService;
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        return _cardEntryService.CardsByIdsAsync(args);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeArgEntity setCode)
    {
        return _cardEntryService.CardsBySetCodeAsync(setCode);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameArgEntity cardName)
    {
        return _cardEntryService.CardsByNameAsync(cardName);
    }

    public Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm)
    {
        return _cardEntryService.CardNameSearchAsync(searchTerm);
    }

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByIdsAsync(ISetIdsArgEntity setIds)
    {
        return _setEntryService.SetsByIdsAsync(setIds);
    }

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesArgEntity setCodes)
    {
        return _setEntryService.SetsByCodeAsync(setCodes);
    }

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
    {
        return _setEntryService.AllSetsAsync();
    }

    public Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm)
    {
        return _artistEntryService.ArtistSearchAsync(searchTerm);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdArgEntity artistId)
    {
        return _artistEntryService.CardsByArtistAsync(artistId);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameArgEntity artistName)
    {
        return _artistEntryService.CardsByArtistNameAsync(artistName);
    }
}
