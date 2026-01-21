using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Entities;

public sealed class AddSetGroupToUserSetCardArgsEntity : IAddSetGroupToUserSetCardArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IAddSetGroupToUserSetCardArgEntity AddSetGroupToUserSetCard { get; init; }
}
