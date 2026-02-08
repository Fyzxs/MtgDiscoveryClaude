using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Actions.Validators;
using Lib.MtgDiscovery.Entry.Commands.UserCards.Mappers;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Abstractions.Identifiers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards;

internal sealed class AddCardToCollectionEntryService : IAddCardToCollectionEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly ICardDomainService _cardDomainService;
    private readonly IAddCardToCollectionArgEntityValidator _addCardToCollectionArgEntityValidator;
    private readonly IAddUserCardArgToItrMapper _addUserCardArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardOufToOutMapper _userCardOufToOutMapper;
    private readonly ICardNameGuidGenerator _cardNameGuidGenerator;
    private readonly IUserCardItrToEnrichedItrMapper _enrichedItrMapper;

    public AddCardToCollectionEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new CardDomainService(logger),
        new AddCardToCollectionArgEntityValidatorContainer(),
        new AddUserCardArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardOufToOutMapper(),
        new CardNameGuidGenerator(),
        new UserCardItrToEnrichedItrMapper())
    { }

    private AddCardToCollectionEntryService(
        IUserCardsDomainService userCardsDomainService,
        ICardDomainService cardDomainService,
        IAddCardToCollectionArgEntityValidator addCardToCollectionArgEntityValidator,
        IAddUserCardArgToItrMapper addUserCardArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardOufToOutMapper userCardOufToOutMapper,
        ICardNameGuidGenerator cardNameGuidGenerator,
        IUserCardItrToEnrichedItrMapper enrichedItrMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _cardDomainService = cardDomainService;
        _addCardToCollectionArgEntityValidator = addCardToCollectionArgEntityValidator;
        _addUserCardArgToItrMapper = addUserCardArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
        _cardNameGuidGenerator = cardNameGuidGenerator;
        _enrichedItrMapper = enrichedItrMapper;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IAddCardToCollectionArgsEntity input, CancellationToken cancellationToken)
    {

        IValidatorActionResult<IOperationResponse<IUserCardOufEntity>> validatorResult = await _addCardToCollectionArgEntityValidator.Validate(input).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
            return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        // Fetch card details first to extract artist_ids and generate card_name_guid
        ICardIdsItrEntity cardIdsItr = new EntryCardIdsItrEntity { CardIds = [input.AddUserCard.CardId] };
        IOperationResponse<ICardItemCollectionOufEntity> cardResponse = await _cardDomainService.CardsByIdsAsync(cardIdsItr, cancellationToken).ConfigureAwait(false);
        if (cardResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(cardResponse.OuterException);

        List<CardItemOutEntity> cards = await _cardItemOufToOutMapper.Map(cardResponse.ResponseData).ConfigureAwait(false);
        if (cards.Count == 0)
            return new FailureOperationResponse<List<CardItemOutEntity>>(new Shared.Invocation.Exceptions.BadRequestOperationException("Card not found"));

        CardItemOutEntity cardItem = cards[0];

        CardNameGuid nameGuid = _cardNameGuidGenerator.GenerateGuid(cardItem.Name);
        string cardNameGuid = nameGuid.AsSystemType().ToString();

        IUserCardItrEntity itrEntity = await _addUserCardArgToItrMapper.Map(input).ConfigureAwait(false);

        IUserCardItrEntity enrichedEntity = _enrichedItrMapper.Map(itrEntity, cardItem, cardNameGuid);

        IOperationResponse<IUserCardOufEntity> addResponse = await _userCardsDomainService.AddUserCardAsync(enrichedEntity, cancellationToken).ConfigureAwait(false);
        if (addResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(addResponse.OuterException);

        // Map the user collection data to the card
        UserCardOutEntity userCardOut = await _userCardOufToOutMapper.Map(addResponse.ResponseData).ConfigureAwait(false);
        cards[0].UserCollection = userCardOut.CollectedList;

        return new SuccessOperationResponse<List<CardItemOutEntity>>(cards);
    }
}
