using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Sets.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Sets;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Sets;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class SetEntryService : ISetEntryService
{
    private readonly ISetDomainService _setDomainService;
    private readonly ISetIdsArgEntityValidator _setIdsArgEntityValidator;
    private readonly ISetCodesArgEntityValidator _setCodesArgEntityValidator;
    private readonly ISetIdsArgToItrMapper _setIdsArgToItrMapper;
    private readonly ISetCodesArgToItrMapper _setCodesArgToItrMapper;
    private readonly ICollectionSetItemOufToOutMapper _setItemOufToOutMapper;

    public SetEntryService(ILogger logger) : this(
        new SetDomainService(logger),
        new SetIdsArgEntityValidatorContainer(),
        new SetCodesArgEntityValidatorContainer(),
        new SetIdsArgToItrMapper(),
        new SetCodesArgToItrMapper(),
        new CollectionSetItemOufToOutMapper())
    { }

    private SetEntryService(
        ISetDomainService setDomainService,
        ISetIdsArgEntityValidator setIdsArgEntityValidator,
        ISetCodesArgEntityValidator setCodesArgEntityValidator,
        ISetIdsArgToItrMapper setIdsArgToItrMapper,
        ISetCodesArgToItrMapper setCodesArgToItrMapper,
        ICollectionSetItemOufToOutMapper setItemOufToOutMapper)
    {
        _setDomainService = setDomainService;
        _setIdsArgEntityValidator = setIdsArgEntityValidator;
        _setCodesArgEntityValidator = setCodesArgEntityValidator;
        _setIdsArgToItrMapper = setIdsArgToItrMapper;
        _setCodesArgToItrMapper = setCodesArgToItrMapper;
        _setItemOufToOutMapper = setItemOufToOutMapper;
    }

    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByIdsAsync(ISetIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionOufEntity>> validatorResult = await _setIdsArgEntityValidator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<ScryfallSetOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetIdsItrEntity itrEntity = await _setIdsArgToItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<ISetItemCollectionOufEntity> opResponse = await _setDomainService.SetsAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<ScryfallSetOutEntity>>(opResponse.OuterException);

        List<ScryfallSetOutEntity> outEntities = await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ScryfallSetOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByCodeAsync(ISetCodesArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionOufEntity>> validatorResult = await _setCodesArgEntityValidator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<ScryfallSetOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetCodesItrEntity itrEntity = await _setCodesArgToItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<ISetItemCollectionOufEntity> opResponse = await _setDomainService.SetsByCodeAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<ScryfallSetOutEntity>>(opResponse.OuterException);

        List<ScryfallSetOutEntity> outEntities = await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ScryfallSetOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> AllSetsAsync()
    {
        IOperationResponse<ISetItemCollectionOufEntity> opResponse = await _setDomainService.AllSetsAsync().ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<ScryfallSetOutEntity>>(opResponse.OuterException);

        List<ScryfallSetOutEntity> outEntities = await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ScryfallSetOutEntity>>(outEntities);
    }
}
