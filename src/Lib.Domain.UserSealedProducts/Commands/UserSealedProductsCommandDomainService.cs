using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Commands;

internal sealed class UserSealedProductsCommandDomainService : IUserSealedProductsCommandDomainService
{
    private readonly IAddUserSealedProductDomainService _addUserSealedProductOperations;

    public UserSealedProductsCommandDomainService(ILogger logger) : this(
        new AddUserSealedProductDomainService(logger))
    { }

    private UserSealedProductsCommandDomainService(
        IAddUserSealedProductDomainService addUserSealedProductOperations) =>
        _addUserSealedProductOperations = addUserSealedProductOperations;

    public Task<IOperationResponse<IUserSealedProductOufEntity>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input) =>
        _addUserSealedProductOperations.Execute(input);
}
