using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class AccessibleCollectionsInquisition : ICosmosInquisition<UserIdExtEntitys>
{
    private readonly ICosmosInquisitor _inquisitor;
    private readonly InquiryDefinition _inquiry;

    public AccessibleCollectionsInquisition(ILogger logger) : this(new CollectionsInquisitor(logger), new AccessibleCollectionsQueryDefinition())
    { }

    private AccessibleCollectionsInquisition(ICosmosInquisitor inquisitor, InquiryDefinition inquiry)
    {
        _inquisitor = inquisitor;
        _inquiry = inquiry;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>([NotNull] UserIdExtEntitys args, CancellationToken cancellationToken = default)
    {
        QueryDefinition query = _inquiry.AsSystemType()
            .WithParameter("@userId", args.UserId);

        OpResponse<IEnumerable<T>> response = await _inquisitor.CrossPartitionQueryAsync<T>(
            query,
            cancellationToken).ConfigureAwait(false);

        return response;
    }
}
