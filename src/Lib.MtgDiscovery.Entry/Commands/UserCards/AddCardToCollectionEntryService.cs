using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Commands.Mappers;
using Lib.MtgDiscovery.Entry.Commands.Validators;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.MtgDiscovery.Entry.Queries.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
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

    public AddCardToCollectionEntryService(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new CardDomainService(logger),
        new AddCardToCollectionArgEntityValidatorContainer(),
        new AddUserCardArgToItrMapper(),
        new CollectionCardItemOufToOutMapper(),
        new UserCardOufToOutMapper())
    { }

    private AddCardToCollectionEntryService(
        IUserCardsDomainService userCardsDomainService,
        ICardDomainService cardDomainService,
        IAddCardToCollectionArgEntityValidator addCardToCollectionArgEntityValidator,
        IAddUserCardArgToItrMapper addUserCardArgToItrMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardOufToOutMapper userCardOufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _cardDomainService = cardDomainService;
        _addCardToCollectionArgEntityValidator = addCardToCollectionArgEntityValidator;
        _addUserCardArgToItrMapper = addUserCardArgToItrMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardOufToOutMapper = userCardOufToOutMapper;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(AddCardToCollectionArgsEntity input)
    {
        IValidatorActionResult<IOperationResponse<IUserCardOufEntity>> validatorResult = await _addCardToCollectionArgEntityValidator.Validate(input.AddUserCard).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        IUserCardItrEntity itrEntity = await _addUserCardArgToItrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IUserCardOufEntity> addResponse = await _userCardsDomainService.AddUserCardAsync(itrEntity).ConfigureAwait(false);
        if (addResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(addResponse.OuterException);

        // Fetch the complete card details
        ICardIdsItrEntity cardIdsItr = new EntryCardIdsItrEntity { CardIds = new[] { addResponse.ResponseData.CardId } };
        IOperationResponse<ICardItemCollectionOufEntity> cardResponse = await _cardDomainService.CardsByIdsAsync(cardIdsItr).ConfigureAwait(false);
        if (cardResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(cardResponse.OuterException);

        List<CardItemOutEntity> cards = await _cardItemOufToOutMapper.Map(cardResponse.ResponseData).ConfigureAwait(false);
        if (cards.Count == 0) return new FailureOperationResponse<List<CardItemOutEntity>>(new Lib.Shared.Invocation.Exceptions.BadRequestOperationException("Card not found"));

        // Map the user collection data to the card
        UserCardOutEntity userCardOut = await _userCardOufToOutMapper.Map(addResponse.ResponseData).ConfigureAwait(false);
        cards[0].UserCollection = userCardOut.CollectedList;

        return new SuccessOperationResponse<List<CardItemOutEntity>>(cards);
    }
}
