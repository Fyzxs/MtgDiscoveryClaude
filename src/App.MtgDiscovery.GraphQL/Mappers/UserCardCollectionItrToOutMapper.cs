using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class UserCardCollectionItrToOutMapper : IUserCardCollectionItrToOutMapper
{
    public Task<UserCardCollectionOutEntity> Map(IUserCardCollectionItrEntity source)
    {
        UserCardCollectionOutEntity result = new()
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = [.. source.CollectedList.Select(item => new CollectedItemOutEntity
            {
                Finish = item.Finish,
                Special = item.Special,
                Count = item.Count
            })]
        };

        return Task.FromResult(result);
    }
}