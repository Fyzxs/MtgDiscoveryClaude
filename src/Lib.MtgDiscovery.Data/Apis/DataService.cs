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

    public DataService(ILogger logger) : this(new CardDataService(logger), new SetDataService())
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
}
