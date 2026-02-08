using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Commands;

internal sealed class UserSealedProductsCommandDomainService : IUserSealedProductsCommandDomainService
{
    private readonly IAddUserSealedProductDomain _addUserSealedProductOperations;

    public UserSealedProductsCommandDomainService(ILogger logger) : this(
        new AddUserSealedProductDomain(logger))
    { }

    private UserSealedProductsCommandDomainService(
        IAddUserSealedProductDomain addUserSealedProductOperations) => _addUserSealedProductOperations = addUserSealedProductOperations;

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken) => await _addUserSealedProductOperations.Execute(input, cancellationToken).ConfigureAwait(false);
}
