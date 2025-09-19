using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Cards;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.Abstractions.Mappers;
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
    private readonly ICardsArgsToItrMapper _cardsArgsToItrMapper;
    private readonly ISetCodeArgsToItrMapper _setCodeArgsToItrMapper;
    private readonly ICardNameArgsToItrMapper _cardNameArgsToItrMapper;
    private readonly ICardSearchTermArgsToItrMapper _cardSearchTermArgsToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly ICollectionCardNameSearchOufToOutMapper _cardNameSearchOufToOutMapper;

    public CardEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new CardIdsArgEntityValidatorContainer(),
        new SetCodeArgEntityValidatorContainer(),
        new CardNameArgEntityValidatorContainer(),
        new CardSearchTermArgEntityValidatorContainer(),
        new CardsArgsToItrMapper(),
        new SetCodeArgsToItrMapper(),
        new CardNameArgsToItrMapper(),
        new CardSearchTermArgsToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new CollectionCardNameSearchOufToOutMapper())
    { }

    private CardEntryService(
        ICardDomainService cardDomainService,
        ICardIdsArgEntityValidator cardIdsArgEntityValidator,
        ISetCodeArgEntityValidator setCodeArgEntityValidator,
        ICardNameArgEntityValidator cardNameArgEntityValidator,
        ICardSearchTermArgEntityValidator cardSearchTermArgEntityValidator,
        ICardsArgsToItrMapper cardsArgsToItrMapper,
        ISetCodeArgsToItrMapper setCodeArgsToItrMapper,
        ICardNameArgsToItrMapper cardNameArgsToItrMapper,
        ICardSearchTermArgsToItrMapper cardSearchTermArgsToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        ICollectionCardNameSearchOufToOutMapper cardNameSearchOufToOutMapper)
    {
        _cardDomainService = cardDomainService;
        _cardIdsArgEntityValidator = cardIdsArgEntityValidator;
        _setCodeArgEntityValidator = setCodeArgEntityValidator;
        _cardNameArgEntityValidator = cardNameArgEntityValidator;
        _cardSearchTermArgEntityValidator = cardSearchTermArgEntityValidator;
        _cardsArgsToItrMapper = cardsArgsToItrMapper;
        _setCodeArgsToItrMapper = setCodeArgsToItrMapper;
        _cardNameArgsToItrMapper = cardNameArgsToItrMapper;
        _cardSearchTermArgsToItrMapper = cardSearchTermArgsToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _cardNameSearchOufToOutMapper = cardNameSearchOufToOutMapper;
    }



    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(ICardIdsArgEntity args)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _cardIdsArgEntityValidator.Validate(args).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardIdsItrEntity itrEntity = await _cardsArgsToItrMapper.Map(args).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsByIdsAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(ISetCodeArgEntity setCode)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _setCodeArgEntityValidator.Validate(setCode).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetCodeItrEntity itrEntity = await _setCodeArgsToItrMapper.Map(setCode).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsBySetCodeAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(ICardNameArgEntity cardName)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _cardNameArgEntityValidator.Validate(cardName).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardNameItrEntity itrEntity = await _cardNameArgsToItrMapper.Map(cardName).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsByNameAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm)
    {
        IValidatorActionResult<IOperationResponse<ICardNameSearchCollectionOufEntity>> validatorResult = await _cardSearchTermArgEntityValidator.Validate(searchTerm).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardNameSearchResultOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardSearchTermItrEntity itrEntity = await _cardSearchTermArgsToItrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<ICardNameSearchCollectionOufEntity> opResponse = await _cardDomainService.CardNameSearchAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardNameSearchResultOutEntity>>(opResponse.OuterException);

        List<CardNameSearchResultOutEntity> outEntities = await _cardNameSearchOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardNameSearchResultOutEntity>>(outEntities);
    }
}
