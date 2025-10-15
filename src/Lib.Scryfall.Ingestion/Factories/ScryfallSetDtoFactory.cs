using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Services;

namespace Lib.Scryfall.Ingestion.Factories;

internal sealed class ScryfallSetDtoFactory : IScryfallDtoFactory<ExtScryfallSetDto>
{
    private readonly ISetGroupingsLoader _groupingsLoader;

    public ScryfallSetDtoFactory() : this(new MonoStateSetGroupingsLoader())
    {
    }

    private ScryfallSetDtoFactory(ISetGroupingsLoader groupingsLoader) => _groupingsLoader = groupingsLoader;

    public ExtScryfallSetDto Create(dynamic data)
    {
        string setCode = data.code;

        // Add groupings if available
        SetGroupingData groupings = _groupingsLoader.GetGroupingsForSet(setCode);
        if (groupings != null)
        {
            // Convert dynamic to JObject to add property
            JObject dataObject = JObject.FromObject(data);
            dataObject["groupings"] = JToken.FromObject(groupings.Groupings);
            data = dataObject;
        }

        return new ExtScryfallSetDto(data);
    }
}
