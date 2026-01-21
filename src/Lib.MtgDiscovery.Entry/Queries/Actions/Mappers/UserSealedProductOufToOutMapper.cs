using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserSealedProductOufToOutMapper : IUserSealedProductOufToOutMapper
{
    public Task<AddUserSealedProductResultOutEntity> Map(IUserSealedProductOufEntity source)
    {
        AddUserSealedProductResultOutEntity result = new()
        {
            ProductUuid = source.ProductUuid,
            Count = source.Count
        };

        return Task.FromResult(result);
    }
}
