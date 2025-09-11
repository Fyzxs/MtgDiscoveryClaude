using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Scribes;
using Lib.Scryfall.Ingestion.Apis.Aggregation;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Lib.Scryfall.Ingestion.Apis.Pipeline;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Pipeline;

internal sealed class TrigramsPipelineService : ITrigramsPipelineService
{
    private readonly ICardNameTrigramAggregator _aggregator;
    private readonly CardNameTrigramsScribe _scribe;
    private readonly IIngestionDashboard _dashboard;
    private readonly ILogger _logger;

    public TrigramsPipelineService(
        ICardNameTrigramAggregator aggregator,
        IIngestionDashboard dashboard,
        ILogger logger)
    {
        _aggregator = aggregator;
        _scribe = new CardNameTrigramsScribe(logger);
        _dashboard = dashboard;
        _logger = logger;
    }

    public void TrackCard(IScryfallCard card)
    {
        _aggregator.Track(card);
    }

    public async Task WriteTrigramsAsync()
    {
        _logger.LogTrigramWritePhaseStarted();

        List<ICardNameTrigramAggregate> trigrams = _aggregator.GetTrigrams().ToList();
        int trigramCount = trigrams.Count;

        if (trigramCount == 0)
        {
            _logger.LogNoTrigramsToWrite();
            return;
        }

        _logger.LogWritingTrigrams(trigramCount);

        int current = 0;
        foreach (ICardNameTrigramAggregate aggregate in trigrams)
        {
            current++;
            string trigram = aggregate.Trigram();
            _dashboard.UpdateProgress("Card Trigrams:", current, trigramCount, "Writing Trigram", trigram);

            CardNameTrigramItem entity = new()
            {
                Trigram = aggregate.Trigram(),
                Cards = new Collection<CardNameTrigramDataItem>(
                    aggregate.Entries().Select(entry => new CardNameTrigramDataItem
                    {
                        Name = entry.Name(),
                        Normalized = entry.Normalized(),
                        Positions = new Collection<int>(entry.Positions().ToList())
                    }).ToList())
            };

            await _scribe.UpsertAsync(entity).ConfigureAwait(false);
        }

        // Clear the aggregator after writing
        _aggregator.Clear();

        _logger.LogTrigramWritePhaseCompleted(trigramCount);
        _dashboard.UpdateCompletedCount("Card Trigrams", trigramCount);
    }
}

internal static partial class TrigramsPipelineServiceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Starting trigram write phase")]
    public static partial void LogTrigramWritePhaseStarted(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Completed writing {TrigramCount} trigram indexes to Cosmos DB")]
    public static partial void LogTrigramWritePhaseCompleted(this ILogger logger, int trigramCount);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "No trigrams to write")]
    public static partial void LogNoTrigramsToWrite(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Writing {TrigramCount} trigram indexes to Cosmos DB")]
    public static partial void LogWritingTrigrams(this ILogger logger, int trigramCount);
}
