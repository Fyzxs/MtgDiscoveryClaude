using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Commands;
using Lib.Domain.UserSealedProducts.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSealedProducts.Apis;

public sealed class UserSealedProductsDomainService : IUserSealedProductsDomainService
{
    private readonly IUserSealedProductsQueryDomainService _queryService;
    private readonly IUserSealedProductsCommandDomainService _commandService;

    public UserSealedProductsDomainService(ILogger logger) : this(
        new UserSealedProductsQueryDomainService(logger),
        new UserSealedProductsCommandDomainService(logger))
    { }

    private UserSealedProductsDomainService(
        IUserSealedProductsQueryDomainService queryService,
        IUserSealedProductsCommandDomainService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    public async Task<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> AddUserSealedProductAsync(IAddUserSealedProductItrEntity input, CancellationToken cancellationToken) => await _commandService.AddUserSealedProductAsync(input, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductOufEntity>>> UserSealedProductsByUserIdAsync(IUserIdItrEntity input, CancellationToken cancellationToken) => await _queryService.UserSealedProductsByUserIdAsync(input, cancellationToken).ConfigureAwait(false);
}
