#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Collections.Tests.Fakes;

public sealed class CollectionsByOwnerAdapterFake : ICollectionsByOwnerAdapter
{
    public IOperationResponse<IEnumerable<CollectionExtEntity>> ExecuteResult { get; set; } =
        new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>([]);

    public bool ShouldReturnFailure { get; set; }

    public int ExecuteInvokeCount { get; private set; }

    public IUserIdXfrEntity? LastExecuteInput { get; private set; }

    public Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> Execute(
        IUserIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        ExecuteInvokeCount++;
        LastExecuteInput = input;

        if (ShouldReturnFailure)
        {
            return Task.FromResult<IOperationResponse<IEnumerable<CollectionExtEntity>>>(
                new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(
                    new CollectionAdapterException("Adapter failed")));
        }

        return Task.FromResult(ExecuteResult);
    }
}
