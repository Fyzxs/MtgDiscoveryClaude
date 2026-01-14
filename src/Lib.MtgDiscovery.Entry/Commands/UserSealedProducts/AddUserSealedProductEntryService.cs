using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.UserSealedProducts.Apis;
using Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserSealedProducts;

internal sealed class AddUserSealedProductEntryService : IAddUserSealedProductEntryService
{
    private readonly IUserSealedProductsCommandDomainService _domainService;
    private readonly IAddSealedProductToCollectionArgsValidator _validator;
    private readonly ISealedProductOufToOutMapper _sealedProductMapper;

    public AddUserSealedProductEntryService(ILogger logger)
        : this(
            new UserSealedProductsDomainService(logger),
            new AddSealedProductToCollectionArgsValidatorContainer(),
            new SealedProductOufToOutMapper())
    {
    }

    private AddUserSealedProductEntryService(
        IUserSealedProductsCommandDomainService domainService,
        IAddSealedProductToCollectionArgsValidator validator,
        ISealedProductOufToOutMapper sealedProductMapper)
    {
        _domainService = domainService;
        _validator = validator;
        _sealedProductMapper = sealedProductMapper;
    }

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> Execute(
        IAddSealedProductToCollectionArgsEntity input)
    {
        IValidatorActionResult<IOperationResponse<List<SealedProductOutEntity>>> validatorResult = await _validator.Validate(input).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) { return validatorResult.FailureStatus(); }

        IAddUserSealedProductItrEntity itrEntity = new AddUserSealedProductItrEntity
        {
            UserId = input.AuthUser.UserId,
            ProductUuid = input.AddUserSealedProduct.ProductUuid,
            SetId = input.AddUserSealedProduct.SetId,
            CountDelta = input.AddUserSealedProduct.UserSealedProductDetails.Count
        };

        IOperationResponse<List<ISealedProductOufEntity>> domainResponse = await _domainService.AddUserSealedProductAsync(itrEntity).ConfigureAwait(false);
        if (domainResponse.IsFailure) { return new FailureOperationResponse<List<SealedProductOutEntity>>(domainResponse.OuterException); }

        SealedProductOutEntity[] mappedEntities = await Task.WhenAll(
            domainResponse.ResponseData.Select(ouf => _sealedProductMapper.Map(ouf))
        ).ConfigureAwait(false);

        return new SuccessOperationResponse<List<SealedProductOutEntity>>(mappedEntities.ToList());
    }
}
