using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Pipeline;

public interface ISetsPipelineService
{
    Task<Dictionary<string, IScryfallSet>> ProcessSetsAsync();
}