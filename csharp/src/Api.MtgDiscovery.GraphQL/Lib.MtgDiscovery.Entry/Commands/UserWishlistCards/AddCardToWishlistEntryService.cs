using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Domain.UserWishlistCards.Apis;
using Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserWishlistCards;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserWishlistCards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Abstractions.Identifiers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Commands.UserWishlistCards;

internal sealed class AddCardToWishlistEntryService : IAddCardToWishlistEntryService
{
    private readonly IUserWishlistCardsDomainService _userWishlistCardsDomainService;
    private readonly ICardDomainService _cardDomainService;
    private readonly IAddCardToWishlistArgEntityValidator _addCardToWishlistArgEntityValidator;
    private readonly IAddUserWishlistCardArgToItrMapper _addUserWishlistCardArgToItrMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserWishlistCardOufToOutMapper _userWishlistCardOufToOutMapper;
    private readonly ICardNameGuidGenerator _cardNameGuidGenerator;

    public AddCardToWishlistEntryService(ILogger logger) : this(
        new UserWishlistCardsDomainService(logger),
        new CardDomainService(logger),
        new AddCardToWishlistArgEntityValidatorContainer(),
        new AddUserWishlistCardArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserWishlistCardOufToOutMapper(),
        new CardNameGuidGenerator())
    { }

    private AddCardToWishlistEntryService(
        IUserWishlistCardsDomainService userWishlistCardsDomainService,
        ICardDomainService cardDomainService,
        IAddCardToWishlistArgEntityValidator addCardToWishlistArgEntityValidator,
        IAddUserWishlistCardArgToItrMapper addUserWishlistCardArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserWishlistCardOufToOutMapper userWishlistCardOufToOutMapper,
        ICardNameGuidGenerator cardNameGuidGenerator)
    {
        _userWishlistCardsDomainService = userWishlistCardsDomainService;
        _cardDomainService = cardDomainService;
        _addCardToWishlistArgEntityValidator = addCardToWishlistArgEntityValidator;
        _addUserWishlistCardArgToItrMapper = addUserWishlistCardArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userWishlistCardOufToOutMapper = userWishlistCardOufToOutMapper;
        _cardNameGuidGenerator = cardNameGuidGenerator;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IAddCardToWishlistArgsEntity input)
    {

        IValidatorActionResult<IOperationResponse<IUserWishlistCardOufEntity>> validatorResult = await _addCardToWishlistArgEntityValidator.Validate(input).ConfigureAwait(false);
        if (validatorResult.IsNotValid())
            return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        ICardIdsItrEntity cardIdsItr = new EntryCardIdsItrEntity { CardIds = [input.AddUserWishlistCard.CardId] };
        IOperationResponse<ICardItemCollectionOufEntity> cardResponse = await _cardDomainService.CardsByIdsAsync(cardIdsItr).ConfigureAwait(false);
        if (cardResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(cardResponse.OuterException);

        List<CardItemOutEntity> cards = await _cardItemOufToOutMapper.Map(cardResponse.ResponseData).ConfigureAwait(false);
        if (cards.Count == 0)
            return new FailureOperationResponse<List<CardItemOutEntity>>(new Shared.Invocation.Exceptions.BadRequestOperationException("Card not found"));

        CardItemOutEntity cardItem = cards[0];
        IEnumerable<string> artistIds = cardItem.ArtistIds ?? [];

        CardNameGuid nameGuid = _cardNameGuidGenerator.GenerateGuid(cardItem.Name);
        string cardNameGuid = nameGuid.AsSystemType().ToString();

        IUserWishlistCardItrEntity itrEntity = await _addUserWishlistCardArgToItrMapper.Map(input).ConfigureAwait(false);

        UserWishlistCardItrEntity enrichedEntity = new()
        {
            UserId = itrEntity.UserId,
            CardId = itrEntity.CardId,
            SetId = itrEntity.SetId,
            CardName = cardItem.Name,
            SetName = cardItem.SetName,
            SetCode = cardItem.SetCode,
            ArtistIds = artistIds,
            CardNameGuid = cardNameGuid,
            Details = itrEntity.Details
        };

        IOperationResponse<IUserWishlistCardOufEntity> addResponse = await _userWishlistCardsDomainService.AddUserWishlistCardAsync(enrichedEntity).ConfigureAwait(false);
        if (addResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(addResponse.OuterException);

        UserWishlistCardOutEntity userWishlistCardOut = await _userWishlistCardOufToOutMapper.Map(addResponse.ResponseData).ConfigureAwait(false);
        cards[0].UserWishlist = userWishlistCardOut.WishlistItems;

        return new SuccessOperationResponse<List<CardItemOutEntity>>(cards);
    }
}
