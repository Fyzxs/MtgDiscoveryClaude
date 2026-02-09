using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserSealedProductOufToOutMapper : IUserSealedProductOufToOutMapper
{
    public Task<UserSealedProductOutEntity> Map(IUserSealedProductOufEntity source)
    {
        UserSealedProductOutEntity result = new()
        {
            UserId = source.UserId,
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
