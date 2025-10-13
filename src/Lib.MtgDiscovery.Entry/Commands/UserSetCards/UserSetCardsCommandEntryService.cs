using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards;

internal sealed class UserSetCardsCommandEntryService : IUserSetCardsCommandEntryService
{
    private readonly IAddSetGroupToUserSetCardEntryService _addSetGroupToUserSetCard;

    public UserSetCardsCommandEntryService(ILogger logger) : this(
        new AddSetGroupToUserSetCardEntryService(logger))
    { }

    private UserSetCardsCommandEntryService(IAddSetGroupToUserSetCardEntryService addSetGroupToUserSetCard)
        => _addSetGroupToUserSetCard = addSetGroupToUserSetCard;

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAuthUserArgEntity authUser, IAddSetGroupToUserSetCardArgEntity argEntity)
        => await _addSetGroupToUserSetCard.Execute(authUser, argEntity).ConfigureAwait(false);
}
