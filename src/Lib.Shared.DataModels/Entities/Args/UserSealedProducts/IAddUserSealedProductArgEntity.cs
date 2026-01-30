using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

public interface IAddUserSealedProductArgEntity : IUserIdArgModel
{
    string ProductUuid { get; }
    string SetId { get; }
    IUserSealedProductDetailsArgEntity UserSealedProductDetails { get; }
}
