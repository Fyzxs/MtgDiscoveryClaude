namespace Lib.Scryfall.Ingestion.Services;

internal sealed class MonoStateSetGroupingsLoader : ISetGroupingsLoader
{
    private static readonly ISetGroupingsLoader s_instance = CreateLoader();

    private static ISetGroupingsLoader CreateLoader() => new SetGroupingsLoader("scryfall_set_groupings.json");

    public SetGroupingData GetGroupingsForSet(string setCode) => s_instance.GetGroupingsForSet(setCode);

    public bool HasGroupingsForSet(string setCode) => s_instance.HasGroupingsForSet(setCode);
}
