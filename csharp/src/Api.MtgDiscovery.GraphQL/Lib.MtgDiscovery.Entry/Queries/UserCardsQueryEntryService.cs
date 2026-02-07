using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.MtgDiscovery.Entry.Queries.UserCards;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class UserCardsQueryEntryService : IUserCardsQueryEntryService
{
    private readonly IUserCardEntryService _userCard;
    private readonly IUserCardsBySetEntryService _userCardsBySet;
    private readonly IUserCardsByIdsEntryService _userCardsByIds;
    private readonly IUserCardsForSigningEntryService _userCardsForSigning;

    public UserCardsQueryEntryService(ILogger logger) : this(
        new UserCardEntryService(logger),
        new UserCardsBySetEntryService(logger),
        new UserCardsByIdsEntryService(logger),
        new UserCardsForSigningEntryService(logger))
    { }

    private UserCardsQueryEntryService(
        IUserCardEntryService userCard,
        IUserCardsBySetEntryService userCardsBySet,
        IUserCardsByIdsEntryService userCardsByIds,
        IUserCardsForSigningEntryService userCardsForSigning)
    {
        _userCard = userCard;
        _userCardsBySet = userCardsBySet;
        _userCardsByIds = userCardsByIds;
        _userCardsForSigning = userCardsForSigning;
    }

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs, CancellationToken cancellationToken) => await _userCard.Execute(cardArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs, CancellationToken cancellationToken) => await _userCardsBySet.Execute(bySetArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs, CancellationToken cancellationToken) => await _userCardsByIds.Execute(cardsArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<SigningResultOutEntity>> UserCardsForSigningAsync(IUserCardsForSigningArgEntity forSigningArgs, CancellationToken cancellationToken) => await _userCardsForSigning.Execute(forSigningArgs, cancellationToken).ConfigureAwait(false);
}
