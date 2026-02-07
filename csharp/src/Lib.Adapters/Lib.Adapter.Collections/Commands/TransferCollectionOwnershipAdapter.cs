using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Commands.Integrators;
using Lib.Adapter.Collections.Commands.Mappers;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Janitors;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Commands;

internal sealed class TransferCollectionOwnershipAdapter : ITransferCollectionOwnershipAdapter
{
    private readonly ITransferOwnershipXfrToReadPointMapper _readPointMapper;
    private readonly ICosmosGopher _gopher;
    private readonly ITransferOwnershipXfrToDeletePointMapper _deletePointMapper;
    private readonly ICosmosContainerDeleteOperator _janitor;
    private readonly ITransferOwnershipIntegrator _integrator;
    private readonly ICosmosScribe _scribe;

    public TransferCollectionOwnershipAdapter(ILogger logger) : this(
        new TransferOwnershipXfrToReadPointMapper(),
        new CollectionGopher(logger),
        new TransferOwnershipXfrToDeletePointMapper(),
        new CollectionJanitor(logger),
        new TransferOwnershipIntegrator(),
        new CollectionScribe(logger))
    { }

    private TransferCollectionOwnershipAdapter(
        ITransferOwnershipXfrToReadPointMapper readPointMapper,
        ICosmosGopher gopher,
        ITransferOwnershipXfrToDeletePointMapper deletePointMapper,
        ICosmosContainerDeleteOperator janitor,
        ITransferOwnershipIntegrator integrator,
        ICosmosScribe scribe)
    {
        _readPointMapper = readPointMapper;
        _gopher = gopher;
        _deletePointMapper = deletePointMapper;
        _janitor = janitor;
        _integrator = integrator;
        _scribe = scribe;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        [NotNull] ITransferCollectionOwnershipXfrEntity input,
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
                new CollectionAdapterException("Cannot transfer ownership of the default collection"));
        }

        DeletePointItem deletePoint = await _deletePointMapper.Map(input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> deleteResponse = await _janitor
            .DeleteAsync<CollectionExtEntity>(deletePoint, cancellationToken).ConfigureAwait(false);

        if (deleteResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Failed to remove collection from original owner: {deleteResponse.StatusCode}"));
        }

        CollectionExtEntity updated = await _integrator.Integrate(existingResponse.Value, input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> upsertResponse = await _scribe
            .UpsertAsync(updated, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Failed to transfer collection {input.CollectionId}: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(upsertResponse.Value);
    }
}
