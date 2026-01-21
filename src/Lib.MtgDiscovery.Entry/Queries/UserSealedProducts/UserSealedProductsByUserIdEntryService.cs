using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Queries;
using Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserSealedProducts;

/// <summary>
/// Entry service for retrieving all sealed products for a specific user.
/// Validates input and delegates to domain layer for data retrieval.
/// </summary>
internal sealed class UserSealedProductsByUserIdEntryService : IUserSealedProductsByUserIdEntryService
{
    private readonly IUserSealedProductsByUserIdDomainService _domainService;
    private readonly IUserIdItrValidator _validator;

    public UserSealedProductsByUserIdEntryService(ILogger logger)
        : this(
            new UserSealedProductsByUserIdDomainService(logger),
            new UserIdItrValidatorContainer())
    {
    }

    private UserSealedProductsByUserIdEntryService(
        IUserSealedProductsByUserIdDomainService domainService,
        IUserIdItrValidator validator)
    {
        _domainService = domainService;
        _validator = validator;
    }

    public async Task<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> Execute(
        string userId)
    {
        IUserIdItrEntity userIdItr = new UserIdItrEntity { UserId = userId };

        IValidatorActionResult<IOperationResponse<IEnumerable<IUserSealedProductItrEntity>>> validatorResult = await _validator.Validate(userIdItr).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return validatorResult.FailureStatus();

        return await _domainService.Execute(userIdItr).ConfigureAwait(false);
    }
}
