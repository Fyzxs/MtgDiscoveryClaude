using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Entities;

public interface IAddSetGroupToUserSetCardArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddSetGroupToUserSetCardArgEntity AddSetGroupToUserSetCard { get; }
}
