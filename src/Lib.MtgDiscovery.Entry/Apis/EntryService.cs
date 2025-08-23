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

    public EntryService(ILogger logger) : this(new CardEntryService(logger), new SetEntryService())
    {

    }
    private EntryService(ICardEntryService cardEntryService, ISetEntryService setEntryService)
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        return _cardEntryService.CardsByIdsAsync(args);
    }
}
