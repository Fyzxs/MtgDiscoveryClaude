using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public interface IUpdateCollectionVisibilityArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IUpdateCollectionVisibilityArgEntity UpdateVisibility { get; }
}
