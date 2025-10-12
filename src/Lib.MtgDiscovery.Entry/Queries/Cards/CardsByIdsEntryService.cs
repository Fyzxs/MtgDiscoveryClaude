using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Cards;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal sealed class CardsByIdsEntryService : ICardsByIdsEntryService
{
    private readonly ICardDomainService _cardDomainService;
    private readonly ICardIdsArgEntityValidator _cardIdsArgEntityValidator;
    private readonly ICardIdsArgToItrMapper _cardIdsArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;

    public CardsByIdsEntryService(ILogger logger) : this(
        new CardDomainService(logger),
        new CardIdsArgEntityValidatorContainer(),
        new CardIdsArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger))
    { }

    private CardsByIdsEntryService(
        ICardDomainService cardDomainService,
        ICardIdsArgEntityValidator cardIdsArgEntityValidator,
        ICardIdsArgToItrMapper cardIdsArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment)
    {
        _cardDomainService = cardDomainService;
        _cardIdsArgEntityValidator = cardIdsArgEntityValidator;
        _cardIdsArgToItrMapper = cardIdsArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(ICardIdsArgEntity args)
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
}
