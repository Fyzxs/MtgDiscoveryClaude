using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Commands.Mappers;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Commands;

internal sealed class DeleteCollectionAdapter : IDeleteCollectionAdapter
{
    private readonly IDeleteCollectionXfrToReadPointMapper _readPointMapper;
    private readonly ICosmosGopher _gopher;
    private readonly IDeleteCollectionXfrToDeletePointMapper _deletePointMapper;
    private readonly ICosmosContainerDeleteOperator _janitor;

    public DeleteCollectionAdapter(ILogger logger) : this(
        new DeleteCollectionXfrToReadPointMapper(),
        new CollectionGopher(logger),
        new DeleteCollectionXfrToDeletePointMapper(),
        new CollectionJanitor(logger))
    { }

    private DeleteCollectionAdapter(
        IDeleteCollectionXfrToReadPointMapper readPointMapper,
        ICosmosGopher gopher,
        IDeleteCollectionXfrToDeletePointMapper deletePointMapper,
        ICosmosContainerDeleteOperator janitor)
    {
        _readPointMapper = readPointMapper;
        _gopher = gopher;
        _deletePointMapper = deletePointMapper;
        _janitor = janitor;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        [NotNull] IDeleteCollectionXfrEntity input,
        CancellationToken cancellationToken)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> existingResponse = await _gopher
            .ReadAsync<CollectionExtEntity>(readPoint, cancellationToken).ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Collection not found: {input.CollectionId}"));
        }

        if (existingResponse.Value.IsDefault)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException("Cannot delete the default collection"));
        }

        DeletePointItem deletePoint = await _deletePointMapper.Map(input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> deleteResponse = await _janitor
            .DeleteAsync<CollectionExtEntity>(deletePoint, cancellationToken).ConfigureAwait(false);

        if (deleteResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Failed to delete collection {input.CollectionId}: {deleteResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(existingResponse.Value);
    }
}
