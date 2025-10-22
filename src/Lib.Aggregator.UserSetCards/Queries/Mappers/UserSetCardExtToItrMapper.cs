using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardExtToItrMapper : IUserSetCardExtToItrMapper
{
    private readonly IUserSetCardGroupExtToItrMapper _groupMapper;
    private readonly IUserSetCardCollectingExtToOufMapper _collectingMapper;

    public UserSetCardExtToItrMapper() : this(new UserSetCardGroupExtToItrMapper(), new UserSetCardCollectingExtToOufMapper())
    { }

    private UserSetCardExtToItrMapper(IUserSetCardGroupExtToItrMapper groupMapper, IUserSetCardCollectingExtToOufMapper collectingMapper)
    {
        _groupMapper = groupMapper;
        _collectingMapper = collectingMapper;
    }

    public async Task<IUserSetCardOufEntity> Map(UserSetCardExtEntity userSetCardExt)
    {
        Dictionary<string, IUserSetCardGroupOufEntity> mappedGroups = [];

        foreach (KeyValuePair<string, UserSetCardGroupExtEntity> group in userSetCardExt.Groups)
        {
            IUserSetCardGroupOufEntity mappedGroup = await _groupMapper.Map(group.Value).ConfigureAwait(false);
            mappedGroups.Add(group.Key, mappedGroup);
        }

        List<IUserSetCardCollectingOufEntity> mappedCollecting = [];

        if (userSetCardExt.Collecting is not null)
        {
            foreach (UserSetCardCollectingExtEntity collecting in userSetCardExt.Collecting)
            {
                IUserSetCardCollectingOufEntity mappedCollectingItem = await _collectingMapper.Map(collecting).ConfigureAwait(false);
                mappedCollecting.Add(mappedCollectingItem);
            }
        }

        return new UserSetCardOufEntity
        {
            UserId = userSetCardExt.UserId,
            SetId = userSetCardExt.SetId,
            TotalCards = userSetCardExt.TotalCards,
            UniqueCards = userSetCardExt.UniqueCards,
            Groups = mappedGroups,
            Collecting = mappedCollecting
        };
    }
}
