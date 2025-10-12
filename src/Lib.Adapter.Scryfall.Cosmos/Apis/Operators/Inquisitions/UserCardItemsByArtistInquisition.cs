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

public sealed class UserCardItemsByArtistInquisition : ICosmosInquisition<UserCardItemsByArtistExtEntitys>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public UserCardItemsByArtistInquisition(ILogger logger) : this(new UserCardsInquisitor(logger), new UserCardItemsByArtistQueryDefinition())
    { }

    private UserCardItemsByArtistInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>([NotNull] UserCardItemsByArtistExtEntitys args, CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@userId", args.UserId)
            .WithParameter("@artistId", args.ArtistId);

        PartitionKey partitionKey = new(args.UserId);

        OpResponse<IEnumerable<T>> response = await _inquisitor.QueryAsync<T>(
            query,
            partitionKey,
            cancellationToken).ConfigureAwait(false);

        return response;
    }
}
