using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Sets.Exceptions;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.DataModels.Entities.Xfrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Sets.Queries;

/// <summary>
/// Retrieves all sets from Cosmos DB using an inquisition query.
/// </summary>
internal sealed class AllSetsAdapter : IAllSetsAdapter
{
    private readonly ICosmosInquisition _allSetsInquisition;

    public AllSetsAdapter(ILogger logger) : this(new AllSetItemsInquisition(logger)) { }

    private AllSetsAdapter(ICosmosInquisition allSetsInquisition) =>
        _allSetsInquisition = allSetsInquisition;

    public async Task<IOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>> Execute(INoArgsXfrEntity input)
    {
        OpResponse<IEnumerable<ScryfallSetItemExtEntity>> response = await _allSetsInquisition
            .QueryAsync<ScryfallSetItemExtEntity>(CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>(
                new SetAdapterException("Failed to retrieve all sets", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<ScryfallSetItemExtEntity>>(response.Value);
    }
}
