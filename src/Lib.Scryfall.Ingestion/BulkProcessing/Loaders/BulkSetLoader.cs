using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Loaders;

internal sealed class BulkSetLoader : IBulkSetLoader
{
    public async Task LoadSetsAsync(IAsyncEnumerable<IScryfallSet> sets, IBulkSetStorage setStorage)
    {
        await foreach (IScryfallSet set in sets.ConfigureAwait(false))
        {
            setStorage.Add(set);
        }
    }
}