using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Commands.Integrators;
using Lib.Adapter.Collections.Commands.Mappers;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Commands;

internal sealed class RenameCollectionAdapter : IRenameCollectionAdapter
{
    private readonly IRenameCollectionXfrToReadPointMapper _readPointMapper;
    private readonly ICosmosGopher _collectionGopher;
    private readonly IRenameCollectionIntegrator _integrator;
    private readonly ICosmosScribe _collectionScribe;

    public RenameCollectionAdapter(ILogger logger) : this(
        new RenameCollectionXfrToReadPointMapper(),
        new CollectionGopher(logger),
        new RenameCollectionIntegrator(),
        new CollectionScribe(logger))
    { }

    private RenameCollectionAdapter(
        IRenameCollectionXfrToReadPointMapper readPointMapper,
        ICosmosGopher collectionGopher,
        IRenameCollectionIntegrator integrator,
        ICosmosScribe collectionScribe)
    {
        _readPointMapper = readPointMapper;
        _collectionGopher = collectionGopher;
        _integrator = integrator;
        _collectionScribe = collectionScribe;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        [NotNull] IRenameCollectionXfrEntity input,
        CancellationToken cancellationToken)
    {
        ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> existingResponse = await _collectionGopher
            .ReadAsync<CollectionExtEntity>(readPoint, cancellationToken).ConfigureAwait(false);

        if (existingResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Collection not found: {input.CollectionId}"));
        }

        CollectionExtEntity updated = await _integrator
            .Integrate(existingResponse.Value, input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> upsertResponse = await _collectionScribe
            .UpsertAsync(updated, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Failed to rename collection {input.CollectionId}: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(upsertResponse.Value);
    }
}
