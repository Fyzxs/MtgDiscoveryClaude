using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Queries;

internal sealed class UserSealedProductsQueryDomainService : IUserSealedProductsQueryDomainService
{
    private readonly IUserSealedProductsByUserIdDomainService _userSealedProductsByUserIdOperations;

    public UserSealedProductsQueryDomainService(ILogger logger) : this(
        new UserSealedProductsByUserIdDomainService(logger))
    { }

    private UserSealedProductsQueryDomainService(
        IUserSealedProductsByUserIdDomainService userSealedProductsByUserIdOperations) => _userSealedProductsByUserIdOperations = userSealedProductsByUserIdOperations;

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductOufEntity>>> UserSealedProductsByUserIdAsync(IUserIdItrEntity input, CancellationToken cancellationToken) => await _userSealedProductsByUserIdOperations.Execute(input, cancellationToken).ConfigureAwait(false);
}
