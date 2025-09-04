using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IAuthUserArgsToItrMapper
{
    Task<IUserInfoItrEntity> Map(IAuthUserArgEntity args);
}