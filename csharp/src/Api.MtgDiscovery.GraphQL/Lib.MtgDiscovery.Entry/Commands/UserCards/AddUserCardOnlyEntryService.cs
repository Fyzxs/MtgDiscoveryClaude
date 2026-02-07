using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Actions.Validators;
using Lib.MtgDiscovery.Entry.Commands.Entities;
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

/// <summary>
/// Entry service for adding user cards without updating UserSetCards.
/// Used by migration tools to separately track individual cards and set-level aggregation.
/// </summary>
internal sealed class AddUserCardOnlyEntryService : IAddUserCardOnlyEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly ICardDomainService _cardDomainService;
    private readonly IAddCardToCollectionArgEntityValidator _addCardToCollectionArgEntityValidator;
    private readonly IAddUserCardArgToItrMapper _addUserCardArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardOufToOutMapper _userCardOufToOutMapper;
    private readonly ICardNameGuidGenerator _cardNameGuidGenerator;

    public AddUserCardOnlyEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new CardDomainService(logger),
        new AddCardToCollectionArgEntityValidatorContainer(),
        new AddUserCardArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardOufToOutMapper(),
        new CardNameGuidGenerator())
    { }

    private AddUserCardOnlyEntryService(
        IUserCardsDomainService userCardsDomainService,
        ICardDomainService cardDomainService,
        IAddCardToCollectionArgEntityValidator addCardToCollectionArgEntityValidator,
        IAddUserCardArgToItrMapper addUserCardArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardOufToOutMapper userCardOufToOutMapper,
        ICardNameGuidGenerator cardNameGuidGenerator)
    {
        _userCardsDomainService = userCardsDomainService;
        _cardDomainService = cardDomainService;
        _addCardToCollectionArgEntityValidator = addCardToCollectionArgEntityValidator;
        _addUserCardArgToItrMapper = addUserCardArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
        _cardNameGuidGenerator = cardNameGuidGenerator;
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
            return new FailureOperationResponse<List<CardItemOutEntity>>(new Lib.Shared.Invocation.Exceptions.BadRequestOperationException("Card not found"));

        // Extract card metadata
        CardItemOutEntity cardItem = cards[0];
        IEnumerable<string> artistIds = cardItem.ArtistIds ?? [];

        // Generate deterministic GUID from card name (matches CardsByName collection)
        CardNameGuid nameGuid = _cardNameGuidGenerator.GenerateGuid(cardItem.Name);
        string cardNameGuid = nameGuid.AsSystemType().ToString();

        // Map user card args to ITR entity with card metadata
        IUserCardItrEntity itrEntity = await _addUserCardArgToItrMapper.Map(input).ConfigureAwait(false);

        // Create updated entity with artist metadata, card name GUID, and denormalized display data
        UserCardCollectionItrEntity enrichedEntity = new()
        {
            UserId = itrEntity.UserId,
            CardId = itrEntity.CardId,
            SetId = itrEntity.SetId,
            CardName = cardItem.Name,
            SetName = cardItem.SetName,
            SetCode = cardItem.SetCode,
            ReleasedAt = cardItem.ReleasedAt,
            Artist = cardItem.Artist,
            ArtistIds = artistIds,
            CardNameGuid = cardNameGuid,
            Details = itrEntity.Details,
            ReplaceMode = itrEntity.ReplaceMode
        };

        IOperationResponse<IUserCardOufEntity> addResponse = await _userCardsDomainService.AddUserCardOnlyAsync(enrichedEntity, cancellationToken).ConfigureAwait(false);
        if (addResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(addResponse.OuterException);

        // Map the user collection data to the card
        UserCardOutEntity userCardOut = await _userCardOufToOutMapper.Map(addResponse.ResponseData).ConfigureAwait(false);
        cards[0].UserCollection = userCardOut.CollectedList;

        return new SuccessOperationResponse<List<CardItemOutEntity>>(cards);
    }
}
