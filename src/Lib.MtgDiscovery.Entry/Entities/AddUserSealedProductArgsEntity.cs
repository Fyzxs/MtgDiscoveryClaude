using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Entities;

public sealed class AddUserSealedProductArgsEntity : IAddUserSealedProductArgsEntity
{
    public IAuthUserArgEntity AuthUser { get; init; }
    public IAddUserSealedProductArgEntity AddUserSealedProduct { get; init; }
}
