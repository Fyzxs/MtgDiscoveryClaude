using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Sets.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Sets;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Sets;

internal sealed class SetsByCodeEntryService : ISetsByCodeEntryService
{
    private readonly ISetDomainService _setDomainService;
    private readonly ISetCodesArgEntityValidator _setCodesArgEntityValidator;
    private readonly ISetCodesArgToItrMapper _setCodesArgToItrMapper;
    private readonly ICollectionSetItemOufToOutMapper _setItemOufToOutMapper;

    public SetsByCodeEntryService(ILogger logger) : this(
        new SetDomainService(logger),
        new SetCodesArgEntityValidatorContainer(),
        new SetCodesArgToItrMapper(),
        new CollectionSetItemOufToOutMapper())
    { }

    private SetsByCodeEntryService(
        ISetDomainService setDomainService,
        ISetCodesArgEntityValidator setCodesArgEntityValidator,
        ISetCodesArgToItrMapper setCodesArgToItrMapper,
        ICollectionSetItemOufToOutMapper setItemOufToOutMapper)
    {
        _setDomainService = setDomainService;
        _setCodesArgEntityValidator = setCodesArgEntityValidator;
        _setCodesArgToItrMapper = setCodesArgToItrMapper;
        _setItemOufToOutMapper = setItemOufToOutMapper;
    }

    public async Task<IOperationResponse<List<SetItemOutEntity>>> Execute(ISetCodesArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionOufEntity>> validatorResult = await _setCodesArgEntityValidator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<SetItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetCodesItrEntity itrEntity = await _setCodesArgToItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<ISetItemCollectionOufEntity> opResponse = await _setDomainService.SetsByCodeAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<SetItemOutEntity>>(opResponse.OuterException);

        List<SetItemOutEntity> outEntities = await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<SetItemOutEntity>>(outEntities);
    }
}
