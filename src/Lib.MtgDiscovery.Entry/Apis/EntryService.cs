using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

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

    public Task<OperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        return _cardEntryService.CardsByIdsAsync(args);
    }
}
