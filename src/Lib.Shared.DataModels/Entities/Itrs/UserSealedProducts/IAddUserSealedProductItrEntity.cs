namespace Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

public interface IAddUserSealedProductItrEntity : Abstractions.IItrEntity
{
    string UserId { get; }
    string ProductUuid { get; }
    string SetId { get; }
    int CountDelta { get; }
}
