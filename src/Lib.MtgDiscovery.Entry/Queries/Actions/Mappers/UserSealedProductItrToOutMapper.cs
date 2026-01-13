using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserSealedProductItrToOutMapper : IUserSealedProductItrToOutMapper
{
    public Task<UserSealedProductOutEntity> Map(IUserSealedProductItrEntity source)
    {
        UserSealedProductOutEntity result = new()
        {
            CollectionId = source.CollectionId,
            ProductUuid = source.ProductUuid,
            ProductName = source.ProductName,
            SetCode = source.SetCode,
            Category = source.Category,
            ImageUrl = source.ImageUrl,
            Count = source.Count,
            UpdatedAt = source.UpdatedAt
        };

        return Task.FromResult(result);
    }
}
