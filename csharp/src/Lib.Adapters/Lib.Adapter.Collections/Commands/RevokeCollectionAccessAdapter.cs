using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

internal sealed class RevokeCollectionAccessAdapter : IRevokeCollectionAccessAdapter
{
    private readonly IRevokeAccessXfrToReadPointMapper _readPointMapper;
    private readonly ICosmosGopher _gopher;
    private readonly IRevokeAccessIntegrator _integrator;
    private readonly ICosmosScribe _scribe;

    public RevokeCollectionAccessAdapter(ILogger logger) : this(
        new RevokeAccessXfrToReadPointMapper(),
        new CollectionGopher(logger),
        new RevokeAccessIntegrator(),
        new CollectionScribe(logger))
    { }

    private RevokeCollectionAccessAdapter(
        IRevokeAccessXfrToReadPointMapper readPointMapper,
        ICosmosGopher gopher,
        IRevokeAccessIntegrator integrator,
        ICosmosScribe scribe)
    {
        _readPointMapper = readPointMapper;
        _gopher = gopher;
        _integrator = integrator;
        _scribe = scribe;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        [NotNull] IRevokeCollectionAccessXfrEntity input,
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

        bool hasUser = existingResponse.Value.AuthorizedUsers.Any(u => u.UserId == input.TargetUserId);

        if (hasUser is false)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"User {input.TargetUserId} is not authorized on collection {input.CollectionId}"));
        }

        CollectionExtEntity updated = await _integrator.Integrate(existingResponse.Value, input).ConfigureAwait(false);

        OpResponse<CollectionExtEntity> upsertResponse = await _scribe
            .UpsertAsync(updated, cancellationToken).ConfigureAwait(false);

        if (upsertResponse.IsNotSuccessful())
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new CollectionAdapterException($"Failed to revoke collection access {input.CollectionId}: {upsertResponse.StatusCode}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(upsertResponse.Value);
    }
}
