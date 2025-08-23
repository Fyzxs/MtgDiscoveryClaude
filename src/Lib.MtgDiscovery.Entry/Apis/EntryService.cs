using System.Threading.Tasks;
using Lib.Shared.DataModels.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public sealed class EntryService : IEntryService
{
    private readonly ICardEntryService _cardEntryService;
    private readonly ISetEntryService _setEntryService;

    public EntryService(ICardEntryService cardEntryService, ISetEntryService setEntryService)
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
    }

    public Task<OperationStatus> CardsByIdsAsync(ICardIdsArgsEntity args)
    {
        return _cardEntryService.CardsByIdsAsync(args);
    }
}
