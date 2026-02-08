using System.Threading.Tasks;
using Lib.Adapter.UserSealedProducts.Apis.Entities;
using Lib.Aggregator.UserSealedProducts.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.Aggregator.UserSealedProducts.Commands.Mappers;

internal sealed class AddUserSealedProductItrToXfrMapper : IAddUserSealedProductItrToXfrMapper
{
    public Task<IUserSealedProductXfrEntity> Map(IAddUserSealedProductItrEntity source)
    {
        IUserSealedProductXfrEntity xfrEntity = new UserSealedProductXfrEntity
        {
            UserId = source.UserId,
            ProductUuid = source.ProductUuid,
            SetId = source.SetId,
            CountDelta = source.CountDelta
        };
        return Task.FromResult(xfrEntity);
    }
}
