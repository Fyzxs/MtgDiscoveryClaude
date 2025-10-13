using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Entities;

public interface IAddCardToCollectionArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddUserCardArgEntity AddUserCard { get; }
}
