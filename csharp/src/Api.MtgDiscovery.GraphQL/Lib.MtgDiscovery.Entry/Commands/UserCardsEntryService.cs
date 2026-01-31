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
    private readonly IAddUserCardOnlyEntryService _addUserCardOnly;

    public UserCardsEntryService(ILogger logger) : this(
        new AddCardToCollectionEntryService(logger),
        new AddUserCardOnlyEntryService(logger))
    { }

    private UserCardsEntryService(
        IAddCardToCollectionEntryService addCardToCollection,
        IAddUserCardOnlyEntryService addUserCardOnly)
    {
        _addCardToCollection = addCardToCollection;
        _addUserCardOnly = addUserCardOnly;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAddCardToCollectionArgsEntity args)
        => await _addCardToCollection.Execute(args).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddUserCardOnlyAsync(IAddCardToCollectionArgsEntity args)
        => await _addUserCardOnly.Execute(args).ConfigureAwait(false);
}
