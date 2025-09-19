using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserCardEnrichment : IUserCardEnrichment
{
    private readonly IUserCardsDomainService _userCardsDomainService;
    private readonly ICollectionCardItemToByIdsItrMapper _collectionCardItemToByIdsItrMapper;
    private readonly ICollectionUserCardDetailsOufToOutMapper _cardDetailsOufToOutMapper;

    public UserCardEnrichment(ILogger logger) : this(
        new UserCardsDomainService(logger),
        new CollectionCardItemToByIdsItrMapper(),
        new CollectionUserCardDetailsOufToOutMapper())
    {

    }

    private UserCardEnrichment(IUserCardsDomainService userCardsDomainService, ICollectionCardItemToByIdsItrMapper collectionCardItemToByIdsItrMapper, ICollectionUserCardDetailsOufToOutMapper collectionUserCardDetailsOufToOutMapper)
    {
        _userCardsDomainService = userCardsDomainService;
        _collectionCardItemToByIdsItrMapper = collectionCardItemToByIdsItrMapper;
        _cardDetailsOufToOutMapper = collectionUserCardDetailsOufToOutMapper;
    }

    public async Task Enrich(List<CardItemOutEntity> outEntities, IUserIdArgEntity args)
    {
        if (args.DoesNotHaveUserId) return;

        IUserCardsByIdsItrEntity userCardsByIdsItrEntity = await _collectionCardItemToByIdsItrMapper.Map(outEntities, args).ConfigureAwait(false);

        IOperationResponse<IEnumerable<IUserCardOufEntity>> userCardResponse = await _userCardsDomainService.UserCardsByIdsAsync(userCardsByIdsItrEntity).ConfigureAwait(false);
        if (userCardResponse.IsFailure) return;

        Dictionary<string, Task<ICollection<CollectedItemOutEntity>>> dictionary = userCardResponse.ResponseData.ToDictionary(
            uc => uc.CardId,
            uc => _cardDetailsOufToOutMapper.Map(uc.CollectedList)
        );

        foreach (CardItemOutEntity card in outEntities)
        {
            if (dictionary.TryGetValue(card.Id, out Task<ICollection<CollectedItemOutEntity>> collectedItems) is false) continue;
            card.UserCollection = await collectedItems.ConfigureAwait(false);
        }
    }
}
