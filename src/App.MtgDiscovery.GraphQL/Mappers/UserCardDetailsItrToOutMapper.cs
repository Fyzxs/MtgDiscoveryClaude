using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class UserCardDetailsItrToOutMapper : IUserCardDetailsItrToOutMapper
{
    public Task<CollectedItemOutEntity> Map(IUserCardDetailsItrEntity source)
    {
        CollectedItemOutEntity result = new()
        {
            Finish = source.Finish,
            Special = source.Special,
            Count = source.Count
        };

        return Task.FromResult(result);
    }
}
