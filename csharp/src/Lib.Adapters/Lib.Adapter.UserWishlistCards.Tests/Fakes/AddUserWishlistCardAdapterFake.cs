#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Adapter.UserWishlistCards.Commands;
using Lib.Adapter.UserWishlistCards.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserWishlistCards.Tests.Fakes;

public sealed class AddUserWishlistCardAdapterFake : IAddUserWishlistCardAdapter
{
    public IOperationResponse<UserWishlistCardExtEntity> ExecuteResult { get; set; } =
        new SuccessOperationResponse<UserWishlistCardExtEntity>(new UserWishlistCardExtEntity
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            WishlistItems = []
        });

    public bool ShouldReturnFailure { get; set; }

    public int ExecuteInvokeCount { get; private set; }

    public IAddUserWishlistCardXfrEntity? LastExecuteInput { get; private set; }

    public Task<IOperationResponse<UserWishlistCardExtEntity>> Execute(
        IAddUserWishlistCardXfrEntity input,
        CancellationToken cancellationToken)
    {
        ExecuteInvokeCount++;
        LastExecuteInput = input;

        if (ShouldReturnFailure)
        {
            return Task.FromResult<IOperationResponse<UserWishlistCardExtEntity>>(
                new FailureOperationResponse<UserWishlistCardExtEntity>(new UserWishlistCardsAdapterException("Adapter failed")));
        }

        return Task.FromResult(ExecuteResult);
    }
}
