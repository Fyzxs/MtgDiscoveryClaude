using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.User;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserInfoOufToOutMapper
{
    Task<UserRegistrationOutEntity> Map(IUserInfoOufEntity userInfo);
}
