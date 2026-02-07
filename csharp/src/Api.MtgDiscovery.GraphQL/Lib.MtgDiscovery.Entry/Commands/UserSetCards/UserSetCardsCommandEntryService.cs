using System.Threading;
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

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardArgsEntity args, CancellationToken cancellationToken)
        => await _addSetGroupToUserSetCard.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddCardToSetAsync(IAddCardToSetArgsEntity args, CancellationToken cancellationToken)
        => await _addCardToSet.Execute(args, cancellationToken).ConfigureAwait(false);
}
