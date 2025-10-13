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

public sealed class ArtistNameTrigramSearchInquisition : ICosmosInquisition<ArtistNameTrigramSearchInquisitionArgs>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public ArtistNameTrigramSearchInquisition(ILogger logger) : this(new ArtistNameTrigramsInquisitor(logger), new ArtistNameTrigramSearchQueryDefinition())
    { }

    private ArtistNameTrigramSearchInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>([NotNull] ArtistNameTrigramSearchInquisitionArgs args, CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@trigram", args.Trigram)
            .WithParameter("@partition", args.Partition)
            .WithParameter("@normalized", args.Normalized);

        PartitionKey partitionKey = new(args.Partition);

        OpResponse<IEnumerable<T>> response = await _inquisitor.QueryAsync<T>(
            query,
            partitionKey,
            cancellationToken).ConfigureAwait(false);

        return response;
    }
}
