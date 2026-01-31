using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserWishlistCards.Queries;

internal sealed class UserWishlistCardsByUserAdapter : IUserWishlistCardsByUserAdapter
{
    private readonly ICosmosInquisition<AllUserWishlistCardsExtEntitys> _inquisition;

    public UserWishlistCardsByUserAdapter(ILogger logger) : this(new AllUserWishlistCardsInquisition(logger)) { }

    private UserWishlistCardsByUserAdapter(ICosmosInquisition<AllUserWishlistCardsExtEntitys> inquisition) => _inquisition = inquisition;

    public async Task<IOperationResponse<IEnumerable<UserWishlistCardExtEntity>>> Execute([NotNull] IUserWishlistCardXfrEntity input)
    {
        AllUserWishlistCardsExtEntitys args = new() { UserId = input.UserId };

        OpResponse<IEnumerable<UserWishlistCardExtEntity>> queryResponse = await _inquisition.QueryAsync<UserWishlistCardExtEntity>(args).ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserWishlistCardExtEntity>>(
                new UserWishlistCardsAdapterException($"Failed to query user wishlist cards: {queryResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<IEnumerable<UserWishlistCardExtEntity>>(queryResponse.Value);
    }
}
