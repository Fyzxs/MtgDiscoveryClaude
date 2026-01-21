using System.Collections.Generic;
using System.Linq;
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

internal sealed class CardsBySetCodeEntryService : ICardsBySetCodeEntryService
{
    private readonly ICardDomainService _cardDomainService;
    private readonly ISetCodeArgEntityValidator _setCodeArgEntityValidator;
    private readonly ISetCodeArgToItrMapper _setCodeArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;

    public CardsBySetCodeEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new SetCodeArgEntityValidatorContainer(),
        new SetCodeArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger))
    { }

    private CardsBySetCodeEntryService(
        ICardDomainService cardDomainService,
        ISetCodeArgEntityValidator setCodeArgEntityValidator,
        ISetCodeArgToItrMapper setCodeArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment)
    {
        _cardDomainService = cardDomainService;
        _setCodeArgEntityValidator = setCodeArgEntityValidator;
        _setCodeArgToItrMapper = setCodeArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(ISetCodeArgEntity setCode)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _setCodeArgEntityValidator.Validate(setCode).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetCodeItrEntity itrEntity = await _setCodeArgToItrMapper.Map(setCode).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsBySetCodeAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        if (setCode.HasUserId)
        {
            // All cards in outEntities are from the same set, extract SetId UUID from first card
            CardItemOutEntity firstCard = outEntities.FirstOrDefault();
            if (firstCard != null && string.IsNullOrEmpty(firstCard.SetId) is false)
            {
                IUserCardsSetItrEntity userCardsSetContext = new UserCardsSetItrEntity
                {
                    UserId = setCode.UserId,
                    SetId = firstCard.SetId
                };
                await _userCardEnrichment.EnrichBySet(outEntities, userCardsSetContext).ConfigureAwait(false);
            }
        }

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }
}
