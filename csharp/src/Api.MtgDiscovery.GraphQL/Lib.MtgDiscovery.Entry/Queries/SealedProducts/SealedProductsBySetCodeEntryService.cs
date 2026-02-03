using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.SealedProducts.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Actions.Validators.SealedProducts;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.SealedProducts;

internal sealed class SealedProductsBySetCodeEntryService : ISealedProductsBySetCodeEntryService
{
    private readonly ISealedProductsDomainService _domainService;
    private readonly ISealedProductsBySetCodeArgEntityValidator _validator;
    private readonly ISealedProductsBySetCodeArgToItrMapper _argToItrMapper;
    private readonly ICollectionSealedProductOufToOutMapper _oufToOutMapper;
    private readonly IUserSealedProductEnrichment _userSealedProductEnrichment;

    public SealedProductsBySetCodeEntryService(ILogger logger) : this(
        new SealedProductsDomainService(logger),
        new SealedProductsBySetCodeArgEntityValidatorContainer(),
        new SealedProductsBySetCodeArgToItrMapper(),
        new CollectionSealedProductOufToOutMapper(),
        new UserSealedProductEnrichment(logger))
    { }

    private SealedProductsBySetCodeEntryService(
        ISealedProductsDomainService domainService,
        ISealedProductsBySetCodeArgEntityValidator validator,
        ISealedProductsBySetCodeArgToItrMapper argToItrMapper,
        ICollectionSealedProductOufToOutMapper oufToOutMapper,
        IUserSealedProductEnrichment userSealedProductEnrichment)
    {
        _domainService = domainService;
        _validator = validator;
        _argToItrMapper = argToItrMapper;
        _oufToOutMapper = oufToOutMapper;
        _userSealedProductEnrichment = userSealedProductEnrichment;
    }

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> Execute(
        ISealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken)
    {
        IValidatorActionResult<IOperationResponse<IEnumerable<ISealedProductOufEntity>>> validatorResult = await _validator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
        {
            return new FailureOperationResponse<List<SealedProductOutEntity>>(validatorResult.FailureStatus().OuterException);
        }

        ISealedProductsBySetCodeItrEntity itrEntity = await _argToItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<IEnumerable<ISealedProductOufEntity>> opResponse = await _domainService.SealedProductsBySetCodeAsync(itrEntity, cancellationToken).ConfigureAwait(false);
        if (opResponse.IsFailure)
        {
            return new FailureOperationResponse<List<SealedProductOutEntity>>(opResponse.OuterException);
        }

        List<SealedProductOutEntity> outEntities = await _oufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        // Enrich with user collection data if collectionId is provided
        await _userSealedProductEnrichment.Enrich(outEntities, args).ConfigureAwait(false);

        return new SuccessOperationResponse<List<SealedProductOutEntity>>(outEntities);
    }
}
