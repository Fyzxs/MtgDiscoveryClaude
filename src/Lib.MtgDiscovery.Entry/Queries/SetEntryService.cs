using System.Threading.Tasks;
using Lib.MtgDiscovery.Data.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class SetEntryService : ISetEntryService
{
    private readonly ISetDataService _setDataService;
    private readonly ISetIdsArgEntityValidator _idsValidator;
    private readonly ISetCodesArgEntityValidator _codesValidator;
    private readonly ISetIdsArgsToItrMapper _idsMapper;
    private readonly ISetCodesArgsToItrMapper _codesMapper;

    public SetEntryService(ILogger logger) : this(
        new DataService(logger),
        new SetIdsArgEntityValidatorContainer(),
        new SetCodesArgEntityValidatorContainer(),
        new SetIdsArgsToItrMapper(),
        new SetCodesArgsToItrMapper())
    { }

    private SetEntryService(
        ISetDataService setDataService,
        ISetIdsArgEntityValidator idsValidator,
        ISetCodesArgEntityValidator codesValidator,
        ISetIdsArgsToItrMapper idsMapper,
        ISetCodesArgsToItrMapper codesMapper)
    {
        _setDataService = setDataService;
        _idsValidator = idsValidator;
        _codesValidator = codesValidator;
        _idsMapper = idsMapper;
        _codesMapper = codesMapper;
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByIdsAsync(ISetIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionItrEntity>> result = await _idsValidator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ISetIdsItrEntity mappedArgs = await _idsMapper.Map(args).ConfigureAwait(false);
        return await _setDataService.SetsAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ISetItemCollectionItrEntity>> result = await _codesValidator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ISetCodesItrEntity mappedArgs = await _codesMapper.Map(args).ConfigureAwait(false);
        return await _setDataService.SetsByCodeAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync()
    {
        return await _setDataService.AllSetsAsync().ConfigureAwait(false);
    }
}
