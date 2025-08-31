using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.BulkProcessing.Storage;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Loaders;

internal interface IBulkSetLoader
{
    Task LoadSetsAsync(IAsyncEnumerable<IScryfallSet> sets, IBulkSetStorage setStorage);
}