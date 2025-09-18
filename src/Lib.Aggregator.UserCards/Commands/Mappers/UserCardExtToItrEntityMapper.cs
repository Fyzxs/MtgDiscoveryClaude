using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal sealed class UserCardExtToItrEntityMapper : IUserCardExtToItrEntityMapper
{
    private readonly IUserCardDetailsExtToItrMapper _detailsMapper;

    public UserCardExtToItrEntityMapper() : this(new UserCardDetailsExtToItrMapper())
    { }

    internal UserCardExtToItrEntityMapper(IUserCardDetailsExtToItrMapper detailsMapper)
    {
        _detailsMapper = detailsMapper;
    }

    public async Task<IUserCardItrEntity> Map([NotNull] UserCardExtEntity source)
    {
        List<IUserCardDetailsItrEntity> collectedList = [];
        foreach (UserCardDetailsExtEntity detail in source.CollectedList)
        {
            IUserCardDetailsItrEntity mappedDetail = await _detailsMapper.Map(detail).ConfigureAwait(false);
            collectedList.Add(mappedDetail);
        }

        return new UserCardItrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = collectedList
        };
    }
}
