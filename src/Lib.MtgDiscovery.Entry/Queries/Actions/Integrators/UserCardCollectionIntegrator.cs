using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;

internal sealed class UserCardCollectionIntegrator : IUserCardCollectionIntegrator
{
    private readonly ICollectionUserCardDetailsOufToOutMapper _cardDetailsOufToOutMapper;

    public UserCardCollectionIntegrator() : this(new CollectionUserCardDetailsOufToOutMapper()) { }

    private UserCardCollectionIntegrator(ICollectionUserCardDetailsOufToOutMapper cardDetailsOufToOutMapper) =>
        _cardDetailsOufToOutMapper = cardDetailsOufToOutMapper;

    public async Task<List<CardItemOutEntity>> Integrate(List<CardItemOutEntity> current, IEnumerable<IUserCardOufEntity> change)
    {
        Dictionary<string, Task<ICollection<CollectedItemOutEntity>>> dictionary = change.ToDictionary(
            uc => uc.CardId,
            uc => _cardDetailsOufToOutMapper.Map(uc.CollectedList)
        );

        foreach (CardItemOutEntity card in current)
        {
            if (dictionary.TryGetValue(card.Id, out Task<ICollection<CollectedItemOutEntity>> collectedItems) is false) continue;
            card.UserCollection = await collectedItems.ConfigureAwait(false);
        }

        return current;
    }
}
