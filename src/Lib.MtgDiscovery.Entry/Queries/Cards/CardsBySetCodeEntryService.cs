using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Cards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
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
    private readonly ISetCodeArgToUserCardsSetContextMapper _setContextMapper;

    public CardsBySetCodeEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new SetCodeArgEntityValidatorContainer(),
        new SetCodeArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger),
        new SetCodeArgToUserCardsSetContextMapper())
    { }

    private CardsBySetCodeEntryService(
        ICardDomainService cardDomainService,
        ISetCodeArgEntityValidator setCodeArgEntityValidator,
        ISetCodeArgToItrMapper setCodeArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment,
        ISetCodeArgToUserCardsSetContextMapper setContextMapper)
    {
        _cardDomainService = cardDomainService;
        _setCodeArgEntityValidator = setCodeArgEntityValidator;
        _setCodeArgToItrMapper = setCodeArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
        _setContextMapper = setContextMapper;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(ISetCodeArgEntity setCode)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _setCodeArgEntityValidator.Validate(setCode).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ISetCodeItrEntity itrEntity = await _setCodeArgToItrMapper.Map(setCode).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _cardDomainService.CardsBySetCodeAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        // Use efficient query by set if userId is present
        if (setCode.HasUserId)
        {
            IUserCardsSetItrEntity userCardsSetContext = await _setContextMapper.Map(setCode, outEntities).ConfigureAwait(false);
            if (userCardsSetContext is not null)
            {
                await _userCardEnrichment.EnrichBySet(outEntities, userCardsSetContext).ConfigureAwait(false);
            }
        }

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }
}
