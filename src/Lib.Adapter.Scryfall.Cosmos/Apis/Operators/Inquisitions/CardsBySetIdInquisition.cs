using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class CardsBySetIdInquisition : ICosmosInquisition<CardsBySetIdInquisitionArgs>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public CardsBySetIdInquisition(ILogger logger) : this(new ScryfallSetCardsInquisitor(logger), new CardsBySetIdQueryDefinition())
    { }

    private CardsBySetIdInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>([NotNull] CardsBySetIdInquisitionArgs args, CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@setId", args.SetId);

        PartitionKey partitionKey = new(args.SetId);

        OpResponse<IEnumerable<T>> response = await _inquisitor.QueryAsync<T>(
            query,
            partitionKey,
            cancellationToken).ConfigureAwait(false);

        return response;
    }
}
