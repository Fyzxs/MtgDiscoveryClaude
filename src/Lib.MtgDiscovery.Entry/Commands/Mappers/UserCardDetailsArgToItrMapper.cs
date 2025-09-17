using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal sealed class UserCardDetailsArgToItrMapper : IUserCardDetailsArgToItrMapper
{
    public Task<IUserCardDetailsItrEntity> Map(IUserCardDetailsArgEntity argItem)
    {
        return Task.FromResult<IUserCardDetailsItrEntity>(new UserCardDetailsItrEntity
        {
            Finish = argItem.Finish,
            Special = argItem.Special,
            Count = argItem.Count
        });
    }
}
