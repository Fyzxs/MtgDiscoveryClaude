using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public sealed class GetCollectionAccessListArgsEntity : IGetCollectionAccessListArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public string CollectionId { get; init; }
}
