using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.MtgDiscovery.Entry.Entities.Collections;

public interface IGetCollectionAccessListArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    string CollectionId { get; }
}
