using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class SetCodeItrEntity : ISetCodeItrEntity
{
    public string SetCode { get; init; }
}