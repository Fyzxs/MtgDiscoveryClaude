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

internal sealed class CardEntryService : ICardEntryService
{
    private readonly ICardDataService _cardDataService;
    private readonly ICardIdsArgEntityValidator _validator;
    private readonly ISetCodeArgEntityValidator _setCodeValidator;
    private readonly ICardsArgsToItrMapper _mapper;
    private readonly ISetCodeArgsToItrMapper _setCodeMapper;

    public CardEntryService(ILogger logger) : this(
        new DataService(logger),
        new CardIdsArgEntityValidatorContainer(),
        new SetCodeArgEntityValidatorContainer(),
        new CardsArgsToItrMapper(),
        new SetCodeArgsToItrMapper())
    {
    }

    private CardEntryService(
        ICardDataService cardDataService,
        ICardIdsArgEntityValidator validator,
        ISetCodeArgEntityValidator setCodeValidator,
        ICardsArgsToItrMapper mapper,
        ISetCodeArgsToItrMapper setCodeMapper)
    {
        _cardDataService = cardDataService;
        _validator = validator;
        _setCodeValidator = setCodeValidator;
        _mapper = mapper;
        _setCodeMapper = setCodeMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> result = await _validator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ICardIdsItrEntity mappedArgs = await _mapper.Map(args).ConfigureAwait(false);
        return await _cardDataService.CardsByIdsAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeArgEntity setCode)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> result = await _setCodeValidator.Validate(setCode).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ISetCodeItrEntity mappedArgs = await _setCodeMapper.Map(setCode).ConfigureAwait(false);
        return await _cardDataService.CardsBySetCodeAsync(mappedArgs).ConfigureAwait(false);
    }
}
