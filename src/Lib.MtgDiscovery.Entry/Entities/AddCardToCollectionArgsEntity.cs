using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class AddCardToCollectionArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IAddUserCardArgEntity AddUserCard { get; init; }
}