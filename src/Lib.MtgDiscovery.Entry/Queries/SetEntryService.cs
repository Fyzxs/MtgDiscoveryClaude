using System.Threading.Tasks;
using Lib.Domain.Sets.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Sets;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class SetEntryService : ISetEntryService
{
    private readonly ISetDomainService _setDomainService;
    private readonly ISetIdsArgEntityValidator _idsValidator;
    private readonly ISetCodesArgEntityValidator _codesValidator;
    private readonly ISetIdsArgsToItrMapper _idsMapper;
    private readonly ISetCodesArgsToItrMapper _codesMapper;

    public SetEntryService(ILogger logger) : this(
        new SetDomainService(logger),
        new SetIdsArgEntityValidatorContainer(),
        new SetCodesArgEntityValidatorContainer(),
        new SetIdsArgsToItrMapper(),
        new SetCodesArgsToItrMapper())
    { }

    private SetEntryService(
        ISetDomainService setDomainService,
        ISetIdsArgEntityValidator idsValidator,
        ISetCodesArgEntityValidator codesValidator,
        ISetIdsArgsToItrMapper idsMapper,
        ISetCodesArgsToItrMapper codesMapper)
    {
        _setDomainService = setDomainService;
        _idsValidator = idsValidator;
        _codesValidator = codesValidator;
        _idsMapper = idsMapper;
        _codesMapper = codesMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByIdsAsync(ISetIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionOufEntity>> result = await _idsValidator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ISetIdsItrEntity mappedArgs = await _idsMapper.Map(args).ConfigureAwait(false);
        return await _setDomainService.SetsAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionOufEntity>> result = await _codesValidator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ISetCodesItrEntity mappedArgs = await _codesMapper.Map(args).ConfigureAwait(false);
        return await _setDomainService.SetsByCodeAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync()
    {
        return await _setDomainService.AllSetsAsync().ConfigureAwait(false);
    }
}
