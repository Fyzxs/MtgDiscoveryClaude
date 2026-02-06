using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Commands.Mappers;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Commands;

internal sealed class CreateCollectionAdapter : ICreateCollectionAdapter
{
    private readonly ICollectionXfrToExtMapper _mapper;
    private readonly ICosmosScribe _collectionScribe;

    public CreateCollectionAdapter(ILogger logger) : this(
        new CollectionXfrToExtMapper(),
        new CollectionScribe(logger))
    { }

    private CreateCollectionAdapter(
        ICollectionXfrToExtMapper mapper,
        ICosmosScribe collectionScribe)
    {
        _mapper = mapper;
        _collectionScribe = collectionScribe;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        [NotNull] ICollectionXfrEntity input,
        CancellationToken cancellationToken)
    {
        CollectionExtEntity extEntity = await _mapper.Map(input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(extEntity, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Failed to create collection {input.CollectionId}: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(upsertResponse.Value);
    }
}
