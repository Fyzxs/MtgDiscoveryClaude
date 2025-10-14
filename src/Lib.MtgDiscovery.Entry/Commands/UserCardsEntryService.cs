using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Commands.UserCards;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands;

internal sealed class UserCardsEntryService : IUserCardsEntryService
{
    private readonly IAddCardToCollectionEntryService _addCardToCollection;

    public UserCardsEntryService(ILogger logger) : this(
        new AddCardToCollectionEntryService(logger))
    { }

    private UserCardsEntryService(IAddCardToCollectionEntryService addCardToCollection)
        => _addCardToCollection = addCardToCollection;

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAddCardToCollectionArgsEntity args)
        => await _addCardToCollection.Execute(args).ConfigureAwait(false);
}
