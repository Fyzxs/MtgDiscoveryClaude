using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class DeleteCollectionArgsEntity : IDeleteCollectionArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public Lib.Shared.DataModels.Entities.Args.Collections.IDeleteCollectionArgEntity DeleteCollection { get; init; }
}
