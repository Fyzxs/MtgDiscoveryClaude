using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Entities;

internal interface IAddSetGroupToUserSetCardArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddSetGroupToUserSetCardArgEntity AddSetGroupToUserSetCard { get; }
}
