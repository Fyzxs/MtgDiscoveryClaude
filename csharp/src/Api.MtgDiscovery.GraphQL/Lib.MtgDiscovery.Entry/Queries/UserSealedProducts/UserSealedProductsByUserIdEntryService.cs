using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Apis;
using Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserSealedProducts;

internal sealed class UserSealedProductsByUserIdEntryService : IUserSealedProductsByUserIdEntryService
{
    private readonly IUserSealedProductsQueryDomainService _domainService;
    private readonly IUserIdItrValidator _validator;

    public UserSealedProductsByUserIdEntryService(ILogger logger)
        : this(
            new UserSealedProductsDomainService(logger),
            new UserIdItrValidatorContainer())
    {
    }

    private UserSealedProductsByUserIdEntryService(
        IUserSealedProductsQueryDomainService domainService,
        IUserIdItrValidator validator)
    {
        _domainService = domainService;
        _validator = validator;
    }

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(
        string userId, CancellationToken cancellationToken)
    {
        IUserIdItrEntity userIdItr = new UserIdItrEntity { UserId = userId };

        IValidatorActionResult<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> validatorResult = await _validator.Validate(userIdItr).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        { return validatorResult.FailureStatus(); }

        return await _domainService.UserSealedProductsByUserIdAsync(userIdItr, cancellationToken).ConfigureAwait(false);
    }
}
