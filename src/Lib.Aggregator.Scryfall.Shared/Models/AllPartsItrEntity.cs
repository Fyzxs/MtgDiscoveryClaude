using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Models;

public sealed class AllPartsItrEntity : IAllPartsItrEntity
{
    private readonly dynamic _data;

    public AllPartsItrEntity(dynamic data)
    {
        _data = data;
    }

    public string ObjectString => _data?["object"];
    public string Id => _data?.id;
    public string Component => _data?.component;
    public string Name => _data?.name;
    public string TypeLine => _data?.type_line;
    public string Uri => _data?.uri;
}