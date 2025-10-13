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

public sealed class CardsByArtistIdInquisition : ICosmosInquisition<CardsByArtistIdInquisitionArgs>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public CardsByArtistIdInquisition(ILogger logger) : this(new ScryfallArtistCardsInquisitor(logger), new CardsByArtistIdQueryDefinition())
    { }

    private CardsByArtistIdInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>([NotNull] CardsByArtistIdInquisitionArgs args, CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@artistId", args.ArtistId);

        PartitionKey partitionKey = new(args.ArtistId);

        OpResponse<IEnumerable<T>> response = await _inquisitor.QueryAsync<T>(
            query,
            partitionKey,
            cancellationToken).ConfigureAwait(false);

        return response;
    }
}
