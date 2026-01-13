using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Entities;

public interface IAddUserSealedProductArgsEntity
{
    IAuthUserArgEntity AuthUser { get; }
    IAddUserSealedProductArgEntity AddUserSealedProduct { get; }
}
