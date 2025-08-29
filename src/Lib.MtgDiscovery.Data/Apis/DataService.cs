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

    public DataService(ILogger logger) : this(new CardDataService(logger), new SetDataService(logger))
    {

    }
    private DataService(ICardDataService cardDataService, ISetDataService setDataService)
    {
        _cardDataService = cardDataService;
        _setDataService = setDataService;
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
}
