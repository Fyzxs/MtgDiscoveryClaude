using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Collections.Apis.Entities;
using Lib.Adapter.Collections.Exceptions;
using Lib.Adapter.Collections.Queries.Mappers;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Collections.Queries;

internal sealed class AccessibleCollectionsAdapter : IAccessibleCollectionsAdapter
{
    private readonly ICosmosInquisition<UserIdExtEntitys> _inquisition;
    private readonly IUserIdXfrToArgsMapper _mapper;

    public AccessibleCollectionsAdapter(ILogger logger) : this(new AccessibleCollectionsInquisition(logger), new UserIdXfrToArgsMapper())
    { }

    private AccessibleCollectionsAdapter(ICosmosInquisition<UserIdExtEntitys> inquisition, IUserIdXfrToArgsMapper mapper)
    {
        _inquisition = inquisition;
        _mapper = mapper;
    }

    public async Task<IOperationResponse<IEnumerable<CollectionExtEntity>>> Execute(
        [NotNull] IUserIdXfrEntity input,
        CancellationToken cancellationToken)
    {
        UserIdExtEntitys args = await _mapper.Map(input).ConfigureAwait(false);

        OpResponse<IEnumerable<CollectionExtEntity>> response = await _inquisition
            .QueryAsync<CollectionExtEntity>(args, cancellationToken)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<CollectionExtEntity>>(
                new CollectionAdapterException($"Failed to query accessible collections for user {input.UserId}", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<CollectionExtEntity>>(response.Value ?? []);
    }
}
