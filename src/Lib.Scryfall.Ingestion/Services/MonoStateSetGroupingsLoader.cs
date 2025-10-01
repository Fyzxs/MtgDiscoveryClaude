namespace Lib.Scryfall.Ingestion.Services;

internal sealed class MonoStateSetGroupingsLoader : ISetGroupingsLoader
{
    private static readonly ISetGroupingsLoader _instance = CreateLoader();

    private static ISetGroupingsLoader CreateLoader() => new SetGroupingsLoader("scryfall_set_groupings.json");

    public SetGroupingData GetGroupingsForSet(string setCode) => _instance.GetGroupingsForSet(setCode);

    public bool HasGroupingsForSet(string setCode) => _instance.HasGroupingsForSet(setCode);
}
