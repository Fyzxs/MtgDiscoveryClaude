namespace Lib.Scryfall.Ingestion.Services;

internal sealed class MonoStateSetGroupingsLoader : ISetGroupingsLoader
{
    private static readonly ISetGroupingsLoader _instance = CreateLoader();

    private static ISetGroupingsLoader CreateLoader()
    {
        return new SetGroupingsLoader("scryfall_set_groupings.json");
    }

    public SetGroupingData GetGroupingsForSet(string setCode)
    {
        return _instance.GetGroupingsForSet(setCode);
    }

    public bool HasGroupingsForSet(string setCode)
    {
        return _instance.HasGroupingsForSet(setCode);
    }
}
