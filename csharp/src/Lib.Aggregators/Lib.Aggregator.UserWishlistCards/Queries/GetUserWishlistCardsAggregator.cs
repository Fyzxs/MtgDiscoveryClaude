using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserWishlistCards.Queries;

internal sealed class GetUserWishlistCardsAggregator : IGetUserWishlistCardsAggregator
{
    private readonly IUserWishlistCardsAdapterService _userWishlistCardsAdapterService;

    public GetUserWishlistCardsAggregator(ILogger logger) : this(
        new UserWishlistCardsAdapterService(logger))
    { }

    private GetUserWishlistCardsAggregator(IUserWishlistCardsAdapterService userWishlistCardsAdapterService) => _userWishlistCardsAdapterService = userWishlistCardsAdapterService;

    public async Task<IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>> Execute(IUserWishlistCardsQueryItrEntity input, CancellationToken cancellationToken)
    {
        UserWishlistCardXfrEntity xfrEntity = new() { UserId = input.UserId };

        IOperationResponse<IEnumerable<UserWishlistCardExtEntity>> response = await _userWishlistCardsAdapterService.UserWishlistCardsByUserAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>(response.OuterException);
        }

        IEnumerable<IUserWishlistCardOufEntity> mappedWishlistCards = response.ResponseData.Select(MapExtToOuf);
        return new SuccessOperationResponse<IEnumerable<IUserWishlistCardOufEntity>>(mappedWishlistCards);
    }

    private static IUserWishlistCardOufEntity MapExtToOuf(UserWishlistCardExtEntity ext)
    {
        return new UserWishlistCardOufEntity
        {
            UserId = ext.UserId,
            CardId = ext.CardId,
            SetId = ext.SetId,
            CardName = ext.CardName,
            SetName = ext.SetName,
            SetCode = ext.SetCode,
            ArtistIds = ext.ArtistIds ?? [],
            CardNameGuid = ext.CardNameGuid,
            CreatedAt = ext.CreatedAt,
            UpdatedAt = ext.UpdatedAt,
            WishlistItems = [.. (ext.WishlistItems ?? []).Select(item => new UserWishlistCardDetailsOufEntity
            {
                Finish = item.Finish,
                Special = item.Special,
                Count = item.Count
            })]
        };
    }

    private sealed class UserWishlistCardXfrEntity : IUserWishlistCardXfrEntity
    {
        public required string UserId { get; init; }
        public string CacheKey => $"user_wishlist_card:{UserId}";
    }

    private sealed class UserWishlistCardOufEntity : IUserWishlistCardOufEntity
    {
        public required string UserId { get; init; }
        public required string CardId { get; init; }
        public required string SetId { get; init; }
        public required string CardName { get; init; }
        public required string SetName { get; init; }
        public required string SetCode { get; init; }
        public required IEnumerable<string> ArtistIds { get; init; }
        public required string CardNameGuid { get; init; }
        public required string CreatedAt { get; init; }
        public required string UpdatedAt { get; init; }
        public required ICollection<IUserWishlistCardDetailsOufEntity> WishlistItems { get; init; }
    }

    private sealed class UserWishlistCardDetailsOufEntity : IUserWishlistCardDetailsOufEntity
    {
        public required string Finish { get; init; }
        public required string Special { get; init; }
        public required int Count { get; init; }
    }
}
