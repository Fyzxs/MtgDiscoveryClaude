using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Cards;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;
namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class CardEntryService : ICardEntryService
{
    private readonly ICardDomainService _cardDomainService;
    private readonly ICardIdsArgEntityValidator _validator;
    private readonly ISetCodeArgEntityValidator _setCodeValidator;
    private readonly ICardNameArgEntityValidator _cardNameValidator;
    private readonly ICardSearchTermArgEntityValidator _searchTermValidator;
    private readonly ICardsArgsToItrMapper _mapper;
    private readonly ISetCodeArgsToItrMapper _setCodeMapper;
    private readonly ICardNameArgsToItrMapper _cardNameMapper;
    private readonly ICardSearchTermArgsToItrMapper _searchTermMapper;

    public CardEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new CardIdsArgEntityValidatorContainer(),
        new SetCodeArgEntityValidatorContainer(),
        new CardNameArgEntityValidatorContainer(),
        new CardSearchTermArgEntityValidatorContainer(),
        new CardsArgsToItrMapper(),
        new SetCodeArgsToItrMapper(),
        new CardNameArgsToItrMapper(),
        new CardSearchTermArgsToItrMapper())
    { }

    private CardEntryService(
        ICardDomainService cardDomainService,
        ICardIdsArgEntityValidator validator,
        ISetCodeArgEntityValidator setCodeValidator,
        ICardNameArgEntityValidator cardNameValidator,
        ICardSearchTermArgEntityValidator searchTermValidator,
        ICardsArgsToItrMapper mapper,
        ISetCodeArgsToItrMapper setCodeMapper,
        ICardNameArgsToItrMapper cardNameMapper,
        ICardSearchTermArgsToItrMapper searchTermMapper)
    {
        _cardDomainService = cardDomainService;
        _validator = validator;
        _setCodeValidator = setCodeValidator;
        _cardNameValidator = cardNameValidator;
        _searchTermValidator = searchTermValidator;
        _mapper = mapper;
        _setCodeMapper = setCodeMapper;
        _cardNameMapper = cardNameMapper;
        _searchTermMapper = searchTermMapper;
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> result = await _validator.Validate(args).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ICardIdsItrEntity mappedArgs = await _mapper.Map(args).ConfigureAwait(false);
        return await _cardDomainService.CardsByIdsAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeArgEntity setCode)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> result = await _setCodeValidator.Validate(setCode).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ISetCodeItrEntity mappedArgs = await _setCodeMapper.Map(setCode).ConfigureAwait(false);
        return await _cardDomainService.CardsBySetCodeAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameArgEntity cardName)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> result = await _cardNameValidator.Validate(cardName).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ICardNameItrEntity mappedArgs = await _cardNameMapper.Map(cardName).ConfigureAwait(false);
        return await _cardDomainService.CardsByNameAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardNameSearchResultCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm)
    {
        IValidatorActionResult<IOperationResponse<ICardNameSearchResultCollectionOufEntity>> result = await _searchTermValidator.Validate(searchTerm).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        ICardSearchTermItrEntity mappedArgs = await _searchTermMapper.Map(searchTerm).ConfigureAwait(false);
        return await _cardDomainService.CardNameSearchAsync(mappedArgs).ConfigureAwait(false);
    }
}
