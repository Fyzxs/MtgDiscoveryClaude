using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class CollectionByIdAdapter : ICollectionByIdAdapter
{
    private readonly ICosmosGopher _gopher;
    private readonly ICosmosInquisition<CollectionIdExtEntity> _inquisition;
    private readonly ICollectionIdXfrToReadPointMapper _readPointMapper;

    public CollectionByIdAdapter(ILogger logger) : this(
        new CollectionGopher(logger),
        new CollectionByIdInquisition(logger),
        new CollectionIdXfrToReadPointMapper())
    { }

    private CollectionByIdAdapter(
        ICosmosGopher gopher,
        ICosmosInquisition<CollectionIdExtEntity> inquisition,
        ICollectionIdXfrToReadPointMapper readPointMapper)
    {
        _gopher = gopher;
        _inquisition = inquisition;
        _readPointMapper = readPointMapper;
    }

    public async Task<IOperationResponse<CollectionExtEntity>> Execute(
        [NotNull] ICollectionIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.OwnerId) is false)
        {
            ReadPointItem readPoint = await _readPointMapper.Map(input).ConfigureAwait(false);

            OpResponse<CollectionExtEntity> pointReadResponse = await _gopher
                .ReadAsync<CollectionExtEntity>(readPoint, cancellationToken)
                .ConfigureAwait(false);

            if (pointReadResponse.IsSuccessful() && pointReadResponse.Value is not null)
            {
                return new SuccessOperationResponse<CollectionExtEntity>(pointReadResponse.Value);
            }
        }

        CollectionIdExtEntity args = new() { CollectionId = input.CollectionId };

        OpResponse<IEnumerable<CollectionExtEntity>> queryResponse = await _inquisition
            .QueryAsync<CollectionExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (queryResponse.IsNotSuccessful() || queryResponse.Value?.Any() is false)
        {
            return new FailureOperationResponse<CollectionExtEntity>(
                new NotFoundOperationException($"Collection not found: {input.CollectionId}"));
        }

        return new SuccessOperationResponse<CollectionExtEntity>(queryResponse.Value!.First());
    }
}
