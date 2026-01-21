using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Containers;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Operators;

internal sealed class DiscoveryCardGopher : CosmosGopher
{
    private readonly ILogger<DiscoveryCardGopher> _logger;

    public DiscoveryCardGopher(ILogger<DiscoveryCardGopher> logger)
        : base(new OldDiscoveryCardsCosmosContainer(logger))
    {
        _logger = logger;
    }

    public async Task<OpResponse<OldDiscoveryCardExtEntity>> ReadCardAsync(string cardId)
    {
        ReadPointItem item = new()
        {
            Id = new ProvidedCosmosItemId(cardId),
            Partition = new ProvidedPartitionKeyValue(cardId)
        };

        OpResponse<OldDiscoveryCardExtEntity> response = await ReadAsync<OldDiscoveryCardExtEntity>(item).ConfigureAwait(false);

        return response;
    }
}
