using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Collections;

[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
internal sealed class ReversibleScryfallSetCollection : IAsyncEnumerable<IScryfallSet>
{
    private readonly IAsyncEnumerable<IScryfallSet> _source;
    private readonly IScryfallIngestionConfiguration _config;
    private readonly ILogger _logger;

    public ReversibleScryfallSetCollection(ILogger logger)
        : this(
            new AllScryfallSetCollection(logger),
            new ConfigScryfallIngestionConfiguration(),
            logger)
    {
    }

    private ReversibleScryfallSetCollection(
        IAsyncEnumerable<IScryfallSet> source,
        IScryfallIngestionConfiguration config,
        ILogger logger)
    {
        _source = source;
        _config = config;
        _logger = logger;
    }

    public async IAsyncEnumerator<IScryfallSet> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        ProcessSetsInReverse processInReverse = _config.ProcessingConfig().ProcessSetsInReverse();
        bool shouldReverse = processInReverse.AsSystemType();

        if (shouldReverse is false)
        {
            await foreach (IScryfallSet set in _source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return set;
            }

            yield break;
        }

        _logger.LogCollectingSetsForReverseProcessing();

        List<IScryfallSet> allSets = [];
        await foreach (IScryfallSet set in _source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            allSets.Add(set);
        }

        _logger.LogReversingSets(allSets.Count);
        allSets.Reverse();

        foreach (IScryfallSet set in allSets)
        {
            yield return set;
        }
    }
}

internal static partial class ReversibleScryfallSetCollectionLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Collecting all sets for reverse processing")]
    public static partial void LogCollectingSetsForReverseProcessing(this ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Reversing {SetCount} sets for chronological processing")]
    public static partial void LogReversingSets(this ILogger logger, int setCount);
}
