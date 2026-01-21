using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class SetCodeItrEntity : ISetCodeItrEntity
{
    public string SetCode { get; init; }
}
