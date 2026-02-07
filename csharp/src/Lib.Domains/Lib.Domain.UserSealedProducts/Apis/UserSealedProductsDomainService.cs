using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Commands;
using Lib.Domain.UserSealedProducts.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Apis;

public sealed class UserSealedProductsDomainService : IUserSealedProductsDomainService
{
    private readonly IUserSealedProductsQueryDomainService _queryOperations;
    private readonly IUserSealedProductsCommandDomainService _commandOperations;

    public UserSealedProductsDomainService(ILogger logger) : this(
        new UserSealedProductsQueryDomainService(logger),
        new UserSealedProductsCommandDomainService(logger))
    { }

    private UserSealedProductsDomainService(
        IUserSealedProductsQueryDomainService queryOperations,
        IUserSealedProductsCommandDomainService commandOperations)
    {
        _queryOperations = queryOperations;
        _commandOperations = commandOperations;
    }

    public async Task<IOperationResponse<List<ISealedProductOufEntity>>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken) => await _commandOperations.AddUserSealedProductAsync(input, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> UserSealedProductsByUserIdAsync(IUserIdItrEntity input, CancellationToken cancellationToken) => await _queryOperations.UserSealedProductsByUserIdAsync(input, cancellationToken).ConfigureAwait(false);
}
