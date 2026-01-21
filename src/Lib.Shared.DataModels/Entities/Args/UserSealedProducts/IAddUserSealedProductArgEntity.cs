namespace Lib.Shared.DataModels.Entities.Args.UserSealedProducts;

public interface IAddUserSealedProductArgEntity
{
    string ProductUuid { get; }
    string SetId { get; }
    string UserId { get; }
    IUserSealedProductDetailsArgEntity UserSealedProductDetails { get; }
}
