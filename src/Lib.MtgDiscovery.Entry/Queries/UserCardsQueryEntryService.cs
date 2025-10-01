using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.UserCards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserCardsQueryEntryService : IUserCardsQueryEntryService
{
    private readonly IUserCardEntryService _userCard;
    private readonly IUserCardsBySetEntryService _userCardsBySet;
    private readonly IUserCardsByIdsEntryService _userCardsByIds;

    public UserCardsQueryEntryService(ILogger logger) : this(
        new UserCardEntryService(logger),
        new UserCardsBySetEntryService(logger),
        new UserCardsByIdsEntryService(logger))
    { }

    private UserCardsQueryEntryService(
        IUserCardEntryService userCard,
        IUserCardsBySetEntryService userCardsBySet,
        IUserCardsByIdsEntryService userCardsByIds)
    {
        _userCard = userCard;
        _userCardsBySet = userCardsBySet;
        _userCardsByIds = userCardsByIds;
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs) => await _userCard.Execute(cardArgs).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs) => await _userCardsBySet.Execute(bySetArgs).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs) => await _userCardsByIds.Execute(cardsArgs).ConfigureAwait(false);
}
