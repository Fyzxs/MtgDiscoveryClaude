using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Commands.Entities;

internal sealed class UserCardDetailsItrEntity : IUserCardDetailsItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
