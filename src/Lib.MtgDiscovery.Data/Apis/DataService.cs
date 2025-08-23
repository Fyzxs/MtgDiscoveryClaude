using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.MtgDiscovery.Data.Apis;

public sealed class DataService : IDataService
{
    private readonly ICardDataService _cardDataService;
    private readonly ISetDataService _setDataService;

    public DataService(ICardDataService cardDataService, ISetDataService setDataService)
    {
        _cardDataService = cardDataService;
        _setDataService = setDataService;
    }

    public Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return _cardDataService.CardsByIdsAsync(args);
    }
}
