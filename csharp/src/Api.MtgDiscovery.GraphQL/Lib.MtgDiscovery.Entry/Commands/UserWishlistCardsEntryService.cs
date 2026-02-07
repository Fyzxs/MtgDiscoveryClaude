using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Commands.UserWishlistCards;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands;

internal sealed class UserWishlistCardsEntryService : IUserWishlistCardsEntryService
{
    private readonly IAddCardToWishlistEntryService _addCardToWishlist;
    private readonly IGetUserWishlistEntryService _getUserWishlist;

    public UserWishlistCardsEntryService(ILogger logger) : this(
        new AddCardToWishlistEntryService(logger),
        new GetUserWishlistEntryService(logger))
    { }

    private UserWishlistCardsEntryService(
        IAddCardToWishlistEntryService addCardToWishlist,
        IGetUserWishlistEntryService getUserWishlist)
    {
        _addCardToWishlist = addCardToWishlist;
        _getUserWishlist = getUserWishlist;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToWishlistAsync(IAddCardToWishlistArgsEntity args, CancellationToken cancellationToken)
        => await _addCardToWishlist.Execute(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> GetUserWishlistAsync(IGetUserWishlistArgsEntity args, CancellationToken cancellationToken)
        => await _getUserWishlist.Execute(args, cancellationToken).ConfigureAwait(false);
}
