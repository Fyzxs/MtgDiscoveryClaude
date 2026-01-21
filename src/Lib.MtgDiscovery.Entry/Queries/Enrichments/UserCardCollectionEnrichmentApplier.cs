using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Enrichments;

internal sealed class UserCardCollectionEnrichmentApplier : IUserCardCollectionEnrichmentApplier
{
    private readonly ICollectionUserCardDetailsOufToOutMapper _cardDetailsOufToOutMapper;

    public UserCardCollectionEnrichmentApplier() : this(new CollectionUserCardDetailsOufToOutMapper()) { }

    private UserCardCollectionEnrichmentApplier(ICollectionUserCardDetailsOufToOutMapper cardDetailsOufToOutMapper) =>
        _cardDetailsOufToOutMapper = cardDetailsOufToOutMapper;

    public async Task Apply(List<CardItemOutEntity> outEntities, IEnumerable<IUserCardOufEntity> userCards)
    {
        Dictionary<string, Task<ICollection<CollectedItemOutEntity>>> dictionary = userCards.ToDictionary(
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
