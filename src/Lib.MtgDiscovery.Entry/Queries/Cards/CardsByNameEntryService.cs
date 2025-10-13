using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Queries.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Cards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal sealed class CardsByNameEntryService : ICardsByNameEntryService
{
    private readonly ICardDomainService _cardDomainService;
    private readonly ICardNameArgEntityValidator _cardNameArgEntityValidator;
    private readonly ICardNameArgToItrMapper _cardNameArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;

    public CardsByNameEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new CardNameArgEntityValidatorContainer(),
        new CardNameArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger))
    { }

    private CardsByNameEntryService(
        ICardDomainService cardDomainService,
        ICardNameArgEntityValidator cardNameArgEntityValidator,
        ICardNameArgToItrMapper cardNameArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment)
    {
        _cardDomainService = cardDomainService;
        _cardNameArgEntityValidator = cardNameArgEntityValidator;
        _cardNameArgToItrMapper = cardNameArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(ICardNameArgEntity cardName)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _cardNameArgEntityValidator.Validate(cardName).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardNameItrEntity itrEntity = await _cardNameArgToItrMapper.Map(cardName).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsByNameAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        // Use efficient query by card name if userId is present
        if (string.IsNullOrEmpty(cardName.UserId) is false)
        {
            IUserCardsNameItrEntity nameContext = new UserCardsNameItrEntity
            {
                UserId = cardName.UserId,
                CardName = cardName.CardName
            };
            await _userCardEnrichment.EnrichByName(outEntities, nameContext).ConfigureAwait(false);
        }

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }
}
