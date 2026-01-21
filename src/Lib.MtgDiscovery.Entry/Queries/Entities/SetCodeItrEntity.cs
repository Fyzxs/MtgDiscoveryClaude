using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class SetCodeItrEntity : ISetCodeItrEntity
{
    public string SetCode { get; init; }
}
