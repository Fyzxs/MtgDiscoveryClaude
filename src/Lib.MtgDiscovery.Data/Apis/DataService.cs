using System.Threading.Tasks;
using Lib.MtgDiscovery.Data.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Data.Apis;

public sealed class DataService : IDataService
{
    private readonly ICardDataService _cardDataService;
    private readonly ISetDataService _setDataService;
    private readonly IArtistDataService _artistDataService;

    public DataService(ILogger logger) : this(new CardDataService(logger), new SetDataService(logger), new ArtistDataService(logger))
    {

    }
    private DataService(ICardDataService cardDataService, ISetDataService setDataService, IArtistDataService artistDataService)
    {
        _cardDataService = cardDataService;
        _setDataService = setDataService;
        _artistDataService = artistDataService;
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return _cardDataService.CardsByIdsAsync(args);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        return _cardDataService.CardsBySetCodeAsync(setCode);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName)
    {
        return _cardDataService.CardsByNameAsync(cardName);
    }

    public Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm)
    {
        return _cardDataService.CardNameSearchAsync(searchTerm);
    }

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity setIds)
    {
        return _setDataService.SetsAsync(setIds);
    }

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes)
    {
        return _setDataService.SetsByCodeAsync(setCodes);
    }

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
    {
        return _setDataService.AllSetsAsync();
    }

    public Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm)
    {
        return _artistDataService.ArtistSearchAsync(searchTerm);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId)
    {
        return _artistDataService.CardsByArtistAsync(artistId);
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName)
    {
        return _artistDataService.CardsByArtistNameAsync(artistName);
    }
}
