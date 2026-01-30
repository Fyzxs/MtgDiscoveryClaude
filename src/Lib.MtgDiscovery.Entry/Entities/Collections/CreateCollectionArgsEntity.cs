using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class CreateCollectionArgsEntity : ICreateCollectionArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public ICreateCollectionArgEntity CreateCollection { get; init; }
}
