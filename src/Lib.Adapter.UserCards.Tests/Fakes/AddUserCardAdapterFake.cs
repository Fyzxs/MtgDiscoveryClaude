#nullable enable
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Commands;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class AddUserCardAdapterFake : IAddUserCardAdapter
{
    public IOperationResponse<UserCardExtEntity> ExecuteResult { get; set; } =
        new SuccessOperationResponse<UserCardExtEntity>(new UserCardExtEntity
        {
            UserId = "user123",
            CardId = "card456",
            SetId = "set789",
            CollectedList = []
        });

    public bool ShouldReturnFailure { get; set; }

    public int ExecuteInvokeCount { get; private set; }

    public IAddUserCardXfrEntity? LastExecuteInput { get; private set; }

    public Task<IOperationResponse<UserCardExtEntity>> Execute(IAddUserCardXfrEntity input)
    {
        ExecuteInvokeCount++;
        LastExecuteInput = input;

        if (ShouldReturnFailure)
        {
            return Task.FromResult<IOperationResponse<UserCardExtEntity>>(
                new FailureOperationResponse<UserCardExtEntity>(new UserCardsAdapterException("Adapter failed")));
        }

        return Task.FromResult(ExecuteResult);
    }
}
