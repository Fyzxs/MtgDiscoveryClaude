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

namespace Lib.Aggregator.UserWishlistCards.Commands;

internal sealed class AddUserWishlistCardAggregator : IAddUserWishlistCardAggregator
{
    private readonly IUserWishlistCardsAdapterService _userWishlistCardsAdapterService;

    public AddUserWishlistCardAggregator(ILogger logger) : this(
        new UserWishlistCardsAdapterService(logger))
    { }

    private AddUserWishlistCardAggregator(
        IUserWishlistCardsAdapterService userWishlistCardsAdapterService) => _userWishlistCardsAdapterService = userWishlistCardsAdapterService;

    public async Task<IOperationResponse<IUserWishlistCardOufEntity>> Execute(IUserWishlistCardItrEntity input, CancellationToken cancellationToken)
    {
        AddUserWishlistCardXfrEntity xfrEntity = MapItrToXfr(input);

        IOperationResponse<UserWishlistCardExtEntity> response = await _userWishlistCardsAdapterService.AddUserWishlistCardAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<IUserWishlistCardOufEntity>(response.OuterException);
        }

        IUserWishlistCardOufEntity mappedWishlistCard = MapExtToOuf(response.ResponseData);
        return new SuccessOperationResponse<IUserWishlistCardOufEntity>(mappedWishlistCard);
    }

    private static AddUserWishlistCardXfrEntity MapItrToXfr(IUserWishlistCardItrEntity itr)
    {
        return new AddUserWishlistCardXfrEntity
        {
            UserId = itr.UserId,
            CardId = itr.CardId,
            SetId = itr.SetId,
            CardName = itr.CardName,
            SetName = itr.SetName,
            SetCode = itr.SetCode,
            ArtistIds = itr.ArtistIds,
            CardNameGuid = itr.CardNameGuid,
            Details = new UserWishlistCardDetailsXfrEntity
            {
                Finish = itr.Details.Finish,
                Special = itr.Details.Special,
                Count = itr.Details.Count
            }
        };
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

    private sealed class AddUserWishlistCardXfrEntity : IAddUserWishlistCardXfrEntity
    {
        public required string UserId { get; init; }
        public required string CardId { get; init; }
        public required string SetId { get; init; }
        public required string CardName { get; init; }
        public required string SetName { get; init; }
        public required string SetCode { get; init; }
        public required IEnumerable<string> ArtistIds { get; init; }
        public required string CardNameGuid { get; init; }
        public required IUserWishlistCardDetailsXfrEntity Details { get; init; }
    }

    private sealed class UserWishlistCardDetailsXfrEntity : IUserWishlistCardDetailsXfrEntity
    {
        public required string Finish { get; init; }
        public required string Special { get; init; }
        public required int Count { get; init; }
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
