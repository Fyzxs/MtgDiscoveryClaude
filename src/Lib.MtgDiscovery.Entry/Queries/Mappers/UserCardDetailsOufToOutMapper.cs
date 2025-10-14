using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class UserCardDetailsOufToOutMapper : IUserCardDetailsOufToOutMapper
{
    public Task<CollectedItemOutEntity> Map(IUserCardDetailsOufEntity source)
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
