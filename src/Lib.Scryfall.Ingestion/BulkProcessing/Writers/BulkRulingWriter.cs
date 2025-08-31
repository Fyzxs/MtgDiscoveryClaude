using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Dtos;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Writers;

internal sealed class BulkRulingWriter : IBulkRulingWriter
{
    private readonly ScryfallRulingItemsScribe _rulingScribe;

    public BulkRulingWriter(ILogger logger)
    {
        _rulingScribe = new ScryfallRulingItemsScribe(logger);
    }

    public async Task WriteRulingsAsync(IEnumerable<BulkRulingsData> rulings)
    {
        List<Task> writeTasks = [];

        foreach (BulkRulingsData rulingData in rulings)
        {
            RulingAggregateData aggregateData = new()
            {
                OracleId = rulingData.OracleId,
                Rulings = rulingData.Rulings.Select(r => new RulingEntry
                {
                    Source = r.Source,
                    PublishedAt = r.PublishedAt,
                    Comment = r.Comment
                })
            };

            ScryfallRulingItem rulingItem = new()
            {
                Data = aggregateData
            };

            writeTasks.Add(_rulingScribe.UpsertAsync(rulingItem));
        }

        await Task.WhenAll(writeTasks).ConfigureAwait(false);
    }
}