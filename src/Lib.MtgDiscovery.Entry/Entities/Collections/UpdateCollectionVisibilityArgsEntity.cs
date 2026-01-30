using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class UpdateCollectionVisibilityArgsEntity : IUpdateCollectionVisibilityArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IUpdateCollectionVisibilityArgEntity UpdateVisibility { get; init; }
}
