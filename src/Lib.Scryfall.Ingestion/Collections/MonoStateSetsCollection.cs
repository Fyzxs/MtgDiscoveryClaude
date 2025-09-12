using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Collections;

internal interface IMonoStateSetsCollection
{
    Task<IScryfallSet> GetSetAsync(string code);
    void Clear();
}

internal sealed class MonoStateSetsCollection : IMonoStateSetsCollection
{
    private static readonly ConcurrentDictionary<string, IScryfallSet> s_sets = new();
    private static readonly SemaphoreSlim s_initLock = new(1, 1);
    private static bool s_initialized;
    private readonly ILogger _logger;

    public MonoStateSetsCollection(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<IScryfallSet> GetSetAsync(string code)
    {
        await EnsureInitializedAsync().ConfigureAwait(false);

        if (s_sets.TryGetValue(code, out IScryfallSet set))
        {
            return set;
        }

        // Return a null object pattern implementation if set not found
        return new NullScryfallSet(code);
    }

    private async Task EnsureInitializedAsync()
    {
        if (s_initialized) return;

        await s_initLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (s_initialized) return;

            // Fetch all sets
            IAsyncEnumerable<IScryfallSet> sets = new FilteredScryfallSetCollection(_logger);

            await foreach (IScryfallSet set in sets.ConfigureAwait(false))
            {
                s_sets[set.Code()] = set;
            }

            s_initialized = true;
            _logger.LogSetsCollectionInitialized(s_sets.Count);
        }
        finally
        {
            s_initLock.Release();
        }
    }

    public void Clear()
    {
        s_sets.Clear();
        s_initialized = false;
    }
}

internal static partial class MonoStateSetsCollectionLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "MonoStateSetsCollection initialized with {Count} sets")]
    public static partial void LogSetsCollectionInitialized(this ILogger logger, int count);
}
