using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSealedProducts.Apis;

public interface IUserSealedProductsCommandAggregatorService
{
    Task<IOperationResponse<IUserSealedProductOufEntity>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input);
}
