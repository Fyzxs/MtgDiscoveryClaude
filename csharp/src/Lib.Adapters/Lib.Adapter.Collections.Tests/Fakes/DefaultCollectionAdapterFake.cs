#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class DefaultCollectionAdapterFake : IDefaultCollectionAdapter
{
    public IOperationResponse<CollectionExtEntity> ExecuteResult { get; set; } =
        new SuccessOperationResponse<CollectionExtEntity>(new CollectionExtEntity
        {
            CollectionId = "col-default",
            OwnerId = "owner-123",
            Name = "Default Collection",
            IsDefault = true
        });

    public bool ShouldReturnFailure { get; set; }

    public int ExecuteInvokeCount { get; private set; }

    public IUserIdXfrEntity? LastExecuteInput { get; private set; }

    public Task<IOperationResponse<CollectionExtEntity>> Execute(
        IUserIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        ExecuteInvokeCount++;
        LastExecuteInput = input;

        if (ShouldReturnFailure)
        {
            return Task.FromResult<IOperationResponse<CollectionExtEntity>>(
                new FailureOperationResponse<CollectionExtEntity>(
                    new CollectionAdapterException("Adapter failed")));
        }

        return Task.FromResult(ExecuteResult);
    }
}
