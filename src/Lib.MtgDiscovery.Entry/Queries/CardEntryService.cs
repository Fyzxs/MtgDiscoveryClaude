using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Cards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class CardEntryService : ICardEntryService
{
    private readonly ICardDomainService _cardDomainService;
    private readonly ICardIdsArgEntityValidator _cardIdsArgEntityValidator;
    private readonly ISetCodeArgEntityValidator _setCodeArgEntityValidator;
    private readonly ICardNameArgEntityValidator _cardNameArgEntityValidator;
    private readonly ICardSearchTermArgEntityValidator _cardSearchTermArgEntityValidator;
    private readonly ICardIdsArgToItrMapper _cardIdsArgToItrMapper;
    private readonly ISetCodeArgToItrMapper _setCodeArgToItrMapper;
    private readonly ICardNameArgToItrMapper _cardNameArgToItrMapper;
    private readonly ICardSearchTermArgToItrMapper _cardSearchTermArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly ICollectionCardNameSearchOufToOutMapper _cardNameSearchOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;

    public CardEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new CardIdsArgEntityValidatorContainer(),
        new SetCodeArgEntityValidatorContainer(),
        new CardNameArgEntityValidatorContainer(),
        new CardSearchTermArgEntityValidatorContainer(),
        new CardIdsArgToItrMapper(),
        new SetCodeArgToItrMapper(),
        new CardNameArgToItrMapper(),
        new CardSearchTermArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new CollectionCardNameSearchOufToOutMapper(),
        new UserCardEnrichment(logger))
    { }

    private CardEntryService(ICardDomainService cardDomainService,
        ICardIdsArgEntityValidator cardIdsArgEntityValidator,
        ISetCodeArgEntityValidator setCodeArgEntityValidator,
        ICardNameArgEntityValidator cardNameArgEntityValidator,
        ICardSearchTermArgEntityValidator cardSearchTermArgEntityValidator,
        ICardIdsArgToItrMapper cardIdsArgToItrMapper,
        ISetCodeArgToItrMapper setCodeArgToItrMapper,
        ICardNameArgToItrMapper cardNameArgToItrMapper,
        ICardSearchTermArgToItrMapper cardSearchTermArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        ICollectionCardNameSearchOufToOutMapper cardNameSearchOufToOutMapper,
        IUserCardEnrichment userCardEnrichment)
    {
        _cardDomainService = cardDomainService;
        _cardIdsArgEntityValidator = cardIdsArgEntityValidator;
        _setCodeArgEntityValidator = setCodeArgEntityValidator;
        _cardNameArgEntityValidator = cardNameArgEntityValidator;
        _cardSearchTermArgEntityValidator = cardSearchTermArgEntityValidator;
        _cardIdsArgToItrMapper = cardIdsArgToItrMapper;
        _setCodeArgToItrMapper = setCodeArgToItrMapper;
        _cardNameArgToItrMapper = cardNameArgToItrMapper;
        _cardSearchTermArgToItrMapper = cardSearchTermArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _cardNameSearchOufToOutMapper = cardNameSearchOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _cardIdsArgEntityValidator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardIdsItrEntity itrEntity = await _cardIdsArgToItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsByIdsAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        await _userCardEnrichment.Enrich(outEntities, args).ConfigureAwait(false);

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(ISetCodeArgEntity setCode)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _setCodeArgEntityValidator.Validate(setCode).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetCodeItrEntity itrEntity = await _setCodeArgToItrMapper.Map(setCode).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsBySetCodeAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(ICardNameArgEntity cardName)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _cardNameArgEntityValidator.Validate(cardName).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardNameItrEntity itrEntity = await _cardNameArgToItrMapper.Map(cardName).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsByNameAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm)
    {
        IValidatorActionResult<IOperationResponse<ICardNameSearchCollectionOufEntity>> validatorResult = await _cardSearchTermArgEntityValidator.Validate(searchTerm).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardNameSearchResultOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardSearchTermItrEntity itrEntity = await _cardSearchTermArgToItrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<ICardNameSearchCollectionOufEntity> opResponse = await _cardDomainService.CardNameSearchAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardNameSearchResultOutEntity>>(opResponse.OuterException);

        List<CardNameSearchResultOutEntity> outEntities = await _cardNameSearchOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardNameSearchResultOutEntity>>(outEntities);
    }
}
