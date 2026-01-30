using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserWishlistCards.Apis;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Queries;

internal sealed class UserWishlistCardsQueryAdapter : IUserWishlistCardsQueryAdapter
{
    private readonly IUserWishlistCardsByUserAdapter _userWishlistCardsByUserAdapter;
    private readonly IUserWishlistCardsByIdsAdapter _userWishlistCardsByIdsAdapter;

    public UserWishlistCardsQueryAdapter(ILogger logger) : this(
        new UserWishlistCardsByUserAdapter(logger),
        new UserWishlistCardsByIdsAdapter(logger))
    { }

    private UserWishlistCardsQueryAdapter(
        IUserWishlistCardsByUserAdapter userWishlistCardsByUserAdapter,
        IUserWishlistCardsByIdsAdapter userWishlistCardsByIdsAdapter)
    {
        _userWishlistCardsByUserAdapter = userWishlistCardsByUserAdapter;
        _userWishlistCardsByIdsAdapter = userWishlistCardsByIdsAdapter;
    }

    public async Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> UserWishlistCardsByUserAsync(IUserWishlistCardXfrEntity userWishlistCard) => await _userWishlistCardsByUserAdapter.Execute(userWishlistCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> UserWishlistCardsByIdsAsync(IUserWishlistCardsByIdsXfrEntity userWishlistCards) => await _userWishlistCardsByIdsAdapter.Execute(userWishlistCards).ConfigureAwait(false);
}
