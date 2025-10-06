using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserSetCards.Entities;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal sealed class UserSetCardExtToItrMapper : IUserSetCardExtToItrMapper
{
    private readonly IUserSetCardGroupExtToItrMapper _groupMapper;

    public UserSetCardExtToItrMapper() : this(new UserSetCardGroupExtToItrMapper())
    { }

    private UserSetCardExtToItrMapper(IUserSetCardGroupExtToItrMapper groupMapper) => _groupMapper = groupMapper;

    public async Task<IUserSetCardOufEntity> Map(UserSetCardExtEntity userSetCardExt)
    {
        Dictionary<string, IUserSetCardGroupOufEntity> mappedGroups = [];

        foreach (KeyValuePair<string, UserSetCardGroupExtEntity> group in userSetCardExt.Groups)
        {
            IUserSetCardGroupOufEntity mappedGroup = await _groupMapper.Map(group.Value).ConfigureAwait(false);
            mappedGroups.Add(group.Key, mappedGroup);
        }

        return new UserSetCardOufEntity
        {
            UserId = userSetCardExt.UserId,
            SetId = userSetCardExt.SetId,
            TotalCards = userSetCardExt.TotalCards,
            UniqueCards = userSetCardExt.UniqueCards,
            Groups = mappedGroups
        };
    }
}
