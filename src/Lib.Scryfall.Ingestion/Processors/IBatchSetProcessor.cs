using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Processors;

internal interface IBatchSetProcessor
{
    Task ProcessSetsAsync(IEnumerable<IScryfallSet> sets);
}