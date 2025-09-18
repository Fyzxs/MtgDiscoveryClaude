using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Internals;

internal sealed class AllPartsItrEntity : IAllPartsItrEntity
{
    private readonly dynamic _data;

    public AllPartsItrEntity(dynamic data) => _data = data;

    public string Id => _data?.id;
    public string Component => _data?.component;
    public string Name => _data?.name;
    public string TypeLine => _data?.type_line;
    public string Uri => _data?.uri;
}
