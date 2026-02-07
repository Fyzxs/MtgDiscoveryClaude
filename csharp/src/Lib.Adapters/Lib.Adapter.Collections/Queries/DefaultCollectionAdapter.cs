using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class DefaultCollectionAdapter : IDefaultCollectionAdapter
{
    private readonly ICollectionsByOwnerAdapter _collectionsAdapter;

    public DefaultCollectionAdapter(ILogger logger) : this(new CollectionsByOwnerAdapter(logger))
    { }

    private DefaultCollectionAdapter(ICollectionsByOwnerAdapter collectionsAdapter)
        => _collectionsAdapter = collectionsAdapter;

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        [NotNull] IUserIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        IOperationResponse<IEnumerable<CollectionExtEntity>> response = await _collectionsAdapter
            .Execute(input, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<CollectionExtEntity>(response.OuterException);
        }

        CollectionExtEntity defaultCollection = response.ResponseData.FirstOrDefault(c => c.IsDefault);

        if (defaultCollection is null)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"No default collection found for user {input.UserId}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(defaultCollection);
    }
}
