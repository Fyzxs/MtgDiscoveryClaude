using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.NewSystem;

internal sealed class NewSystemCardAdder : INewSystemCardAdder
{
    private readonly ILogger _logger;
    private readonly IUserCardsEntryService _userCardsEntryService;

    public NewSystemCardAdder(ILogger logger, IUserCardsEntryService userCardsEntryService)
    {
        _logger = logger;
        _userCardsEntryService = userCardsEntryService;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAddCardToCollectionArgsEntity args)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _userCardsEntryService
            .AddCardToCollectionAsync(args)
            .ConfigureAwait(false);

        return response;
    }
}
