using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class SelectAllSetItemsInquisition : ICosmosInquisition
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public SelectAllSetItemsInquisition(ILogger logger) : this(new ScryfallSetItemsInquisitor(logger), new SelectAllSetItemsQueryDefinition())
    { }

    private SelectAllSetItemsInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(CancellationToken cancellationToken = default)
    {
        OpResponse<IEnumerable<T>> response = await _inquisitor.QueryAsync<T>(
            _inquiry,
            PartitionKey.None,
            cancellationToken).ConfigureAwait(false);

        return response;
    }
}
