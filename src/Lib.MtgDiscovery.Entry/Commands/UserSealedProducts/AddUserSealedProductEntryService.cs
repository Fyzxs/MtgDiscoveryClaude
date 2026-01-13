using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Commands;
using Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserSealedProducts;

/// <summary>
/// Entry service for adding a user sealed product to the collection.
/// Validates input, maps ArgEntity to ItrEntity, and delegates to domain layer.
/// </summary>
internal sealed class AddUserSealedProductEntryService : IAddUserSealedProductEntryService
{
    private readonly IAddUserSealedProductDomainService _domainService;
    private readonly IAddUserSealedProductArgValidator _validator;

    public AddUserSealedProductEntryService(ILogger logger)
        : this(
            new AddUserSealedProductDomainService(logger),
            new AddUserSealedProductArgValidatorContainer())
    {
    }

    private AddUserSealedProductEntryService(
        IAddUserSealedProductDomainService domainService,
        IAddUserSealedProductArgValidator validator)
    {
        _domainService = domainService;
        _validator = validator;
    }

    public async Task<IOperationResponse<IUserSealedProductOufEntity>> Execute(
        IAddUserSealedProductArgEntity input)
    {
        IValidatorActionResult<IOperationResponse<IUserSealedProductOufEntity>> validatorResult = await _validator.Validate(input).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return validatorResult.FailureStatus();

        IAddUserSealedProductItrEntity itrEntity = new AddUserSealedProductItrEntity
        {
            UserId = input.UserId,
            ProductUuid = input.ProductUuid,
            SetId = input.SetId,
            CountDelta = input.CountDelta
        };

        return await _domainService.Execute(itrEntity).ConfigureAwait(false);
    }
}
