using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Pipeline;

public interface IRulingsPipelineService
{
    Task<Dictionary<string, IScryfallRuling>> ProcessRulingsAsync();
}