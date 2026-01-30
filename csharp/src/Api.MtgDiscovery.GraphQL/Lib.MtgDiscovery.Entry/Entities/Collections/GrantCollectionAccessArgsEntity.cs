using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class GrantCollectionAccessArgsEntity : IGrantCollectionAccessArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IGrantCollectionAccessArgEntity GrantAccess { get; init; }
}
