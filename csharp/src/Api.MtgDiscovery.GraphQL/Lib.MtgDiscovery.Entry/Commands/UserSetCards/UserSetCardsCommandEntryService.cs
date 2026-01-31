using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards;

internal sealed class UserSetCardsCommandEntryService : IUserSetCardsCommandEntryService
{
    private readonly IAddSetGroupToUserSetCardEntryService _addSetGroupToUserSetCard;
    private readonly IAddCardToSetEntryService _addCardToSet;

    public UserSetCardsCommandEntryService(ILogger logger) : this(
        new AddSetGroupToUserSetCardEntryService(logger),
        new AddCardToSetEntryService(logger))
    { }

    private UserSetCardsCommandEntryService(
        IAddSetGroupToUserSetCardEntryService addSetGroupToUserSetCard,
        IAddCardToSetEntryService addCardToSet)
    {
        _addSetGroupToUserSetCard = addSetGroupToUserSetCard;
        _addCardToSet = addCardToSet;
    }

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardArgsEntity args)
        => await _addSetGroupToUserSetCard.Execute(args).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddCardToSetAsync(IAddCardToSetArgsEntity args)
        => await _addCardToSet.Execute(args).ConfigureAwait(false);
}
