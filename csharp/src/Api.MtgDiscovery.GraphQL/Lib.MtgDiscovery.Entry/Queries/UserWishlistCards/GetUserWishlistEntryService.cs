using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Domain.UserWishlistCards.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Enrichments;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserWishlistCards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.UserWishlistCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.UserWishlistCards;

internal sealed class GetUserWishlistEntryService : IGetUserWishlistEntryService
{
    private readonly IUserWishlistCardsDomainService _userWishlistCardsDomainService;
    private readonly ICardDomainService _cardDomainService;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;
    private readonly IUserCardEnrichment _userCardEnrichment;
    private readonly IUserWishlistCardByIdsEnrichment _userWishlistCardEnrichment;

    public GetUserWishlistEntryService(ILogger logger) : this(
        new UserWishlistCardsDomainService(logger),
        new CardDomainService(logger),
        new CollectionCardItemOufToOutMapper(),
        new UserCardEnrichment(logger),
        new UserWishlistCardByIdsEnrichment(logger))
    { }

    private GetUserWishlistEntryService(
        IUserWishlistCardsDomainService userWishlistCardsDomainService,
        ICardDomainService cardDomainService,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper,
        IUserCardEnrichment userCardEnrichment,
        IUserWishlistCardByIdsEnrichment userWishlistCardEnrichment)
    {
        _userWishlistCardsDomainService = userWishlistCardsDomainService;
        _cardDomainService = cardDomainService;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
        _userCardEnrichment = userCardEnrichment;
        _userWishlistCardEnrichment = userWishlistCardEnrichment;
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> Execute(IGetUserWishlistArgsEntity input, CancellationToken cancellationToken)
    {
        // Step 1: Get wishlist entries to extract card IDs
        IUserWishlistCardsQueryItrEntity wishlistCardsItr = new UserWishlistCardsQueryItrEntity
        {
            UserId = input.TargetUserId
        };

        IOperationResponse<IEnumerable<IUserWishlistCardOufEntity>> wishlistResponse = await _userWishlistCardsDomainService.GetUserWishlistCardsAsync(wishlistCardsItr, cancellationToken).ConfigureAwait(false);
        if (wishlistResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(wishlistResponse.OuterException);

        // Step 2: Extract card IDs from wishlist entries
        List<string> cardIds = [.. wishlistResponse.ResponseData.Select(w => w.CardId)];
        if (cardIds.Count == 0)
        {
            return new SuccessOperationResponse<List<CardItemOutEntity>>([]);
        }

        // Step 3: Fetch full card data using CardDomainService
        ICardIdsItrEntity cardIdsItr = new CardIdsItrEntity { CardIds = cardIds };
        IOperationResponse<ICardItemCollectionOufEntity> cardsResponse = await _cardDomainService.CardsByIdsAsync(cardIdsItr, cancellationToken).ConfigureAwait(false);
        if (cardsResponse.IsFailure)
            return new FailureOperationResponse<List<CardItemOutEntity>>(cardsResponse.OuterException);

        // Step 4: Map to output entities
        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(cardsResponse.ResponseData).ConfigureAwait(false);

        // Step 5: Apply enrichments (collection and wishlist data)
        IUserIdArgEntity enrichmentArgs = new UserIdArgsEntity { UserId = input.TargetUserId };
        await _userCardEnrichment.Enrich(outEntities, enrichmentArgs, cancellationToken).ConfigureAwait(false);
        await _userWishlistCardEnrichment.Enrich(outEntities, enrichmentArgs, cancellationToken).ConfigureAwait(false);

        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    private sealed class UserWishlistCardsQueryItrEntity : IUserWishlistCardsQueryItrEntity
    {
        public required string UserId { get; init; }
    }

    private sealed class CardIdsItrEntity : ICardIdsItrEntity
    {
        public required ICollection<string> CardIds { get; init; }
    }

    private sealed class UserIdArgsEntity : IUserIdArgEntity
    {
        public required string UserId { get; init; }
    }
}
