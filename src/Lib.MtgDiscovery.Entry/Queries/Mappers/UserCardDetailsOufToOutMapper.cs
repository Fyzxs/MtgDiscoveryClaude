using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserCardDetailsOufToOutMapper : IUserCardDetailsOufToOutMapper
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