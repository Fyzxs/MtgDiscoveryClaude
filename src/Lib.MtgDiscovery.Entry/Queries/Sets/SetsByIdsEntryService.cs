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

internal sealed class SetsByIdsEntryService : ISetsByIdsEntryService
{
    private readonly ISetDomainService _setDomainService;
    private readonly ISetIdsArgEntityValidator _setIdsArgEntityValidator;
    private readonly ISetIdsArgToItrMapper _setIdsArgToItrMapper;
    private readonly ICollectionSetItemOufToOutMapper _setItemOufToOutMapper;

    public SetsByIdsEntryService(ILogger logger) : this(
        new SetDomainService(logger),
        new SetIdsArgEntityValidatorContainer(),
        new SetIdsArgToItrMapper(),
        new CollectionSetItemOufToOutMapper())
    { }

    private SetsByIdsEntryService(
        ISetDomainService setDomainService,
        ISetIdsArgEntityValidator setIdsArgEntityValidator,
        ISetIdsArgToItrMapper setIdsArgToItrMapper,
        ICollectionSetItemOufToOutMapper setItemOufToOutMapper)
    {
        _setDomainService = setDomainService;
        _setIdsArgEntityValidator = setIdsArgEntityValidator;
        _setIdsArgToItrMapper = setIdsArgToItrMapper;
        _setItemOufToOutMapper = setItemOufToOutMapper;
    }

    public async Task<IOperationResponse<List<SetItemOutEntity>>> Execute(ISetIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionOufEntity>> validatorResult = await _setIdsArgEntityValidator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<SetItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetIdsItrEntity itrEntity = await _setIdsArgToItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<ISetItemCollectionOufEntity> opResponse = await _setDomainService.SetsAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<SetItemOutEntity>>(opResponse.OuterException);

        List<SetItemOutEntity> outEntities = await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<SetItemOutEntity>>(outEntities);
    }
}
