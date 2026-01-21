using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace Lib.MtgDiscovery.Entry.Entities;

public interface IAddCardToCollectionArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddUserCardArgEntity AddUserCard { get; }
}
