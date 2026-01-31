using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Entities;

public sealed class AddSealedProductToCollectionArgsEntity : IAddSealedProductToCollectionArgsEntity
{
    public required IAuthUserArgEntity AuthUser { get; init; }
    public required IAddUserSealedProductArgEntity AddUserSealedProduct { get; init; }
}
