using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Scryfall.Ingestion.Services;
using Lib.Scryfall.Shared.Apis.Models;
using Newtonsoft.Json.Linq;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class ScryfallSetToCosmosMapper : IScryfallSetToCosmosMapper
{
    private readonly ISetGroupingsLoader _groupingsLoader;

    public ScryfallSetToCosmosMapper() : this(new MonoStateSetGroupingsLoader())
    {
    }

    private ScryfallSetToCosmosMapper(ISetGroupingsLoader groupingsLoader) => _groupingsLoader = groupingsLoader;

    public Task<ScryfallSetItemExtEntity> Map(IScryfallSet scryfallSet)
    {
        dynamic data = scryfallSet.Data();
        string setCode = scryfallSet.Code();

        // Add groupings if available
        SetGroupingData groupings = _groupingsLoader.GetGroupingsForSet(setCode);
        if (groupings != null)
        {
            // Convert dynamic to JObject to add property
            JObject dataObject = JObject.FromObject(data);
            dataObject["groupings"] = JToken.FromObject(groupings.Groupings);
            data = dataObject;
        }

        ScryfallSetItemExtEntity result = new()
        {
            Data = data
        };

        return Task.FromResult(result);
    }
}
