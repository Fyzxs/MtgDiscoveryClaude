using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Cards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal sealed class CardNameSearchEntryService : ICardNameSearchEntryService
{
    private readonly ICardDomainService _cardDomainService;
    private readonly ICardSearchTermArgEntityValidator _cardSearchTermArgEntityValidator;
    private readonly ICardSearchTermArgToItrMapper _cardSearchTermArgToItrMapper;
    private readonly ICollectionCardNameSearchOufToOutMapper _cardNameSearchOufToOutMapper;

    public CardNameSearchEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new CardSearchTermArgEntityValidatorContainer(),
        new CardSearchTermArgToItrMapper(),
        new CollectionCardNameSearchOufToOutMapper())
    { }

    private CardNameSearchEntryService(
        ICardDomainService cardDomainService,
        ICardSearchTermArgEntityValidator cardSearchTermArgEntityValidator,
        ICardSearchTermArgToItrMapper cardSearchTermArgToItrMapper,
        ICollectionCardNameSearchOufToOutMapper cardNameSearchOufToOutMapper)
    {
        _cardDomainService = cardDomainService;
        _cardSearchTermArgEntityValidator = cardSearchTermArgEntityValidator;
        _cardSearchTermArgToItrMapper = cardSearchTermArgToItrMapper;
        _cardNameSearchOufToOutMapper = cardNameSearchOufToOutMapper;
    }

    public async Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> Execute(ICardSearchTermArgEntity searchTerm)
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