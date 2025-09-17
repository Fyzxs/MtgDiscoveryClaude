using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

/// <summary>
/// Maps UserCardExtEntity to IUserCardItrEntity.
/// </summary>
internal sealed class UserCardExtToItrEntityMapper : IUserCardExtToItrEntityMapper
{
    public UserCardExtToItrEntityMapper()
    { }

    public async Task<IUserCardItrEntity> Map([NotNull] UserCardExtEntity source)
    {
        await Task.CompletedTask.ConfigureAwait(false);

        ICollection<IUserCardDetailsItrEntity> collectedList = source.CollectedList
            .Select(detail => new UserCardDetailsItrEntity
            {
                Finish = detail.Finish,
                Special = detail.Special,
                Count = detail.Count
            })
            .Cast<IUserCardDetailsItrEntity>()
            .ToList();

        return new UserCardItrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = collectedList
        };
    }
}