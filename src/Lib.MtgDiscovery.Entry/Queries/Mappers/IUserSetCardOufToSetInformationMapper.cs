using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserSetCardOufToSetInformationMapper
{
    Task<SetInformationOutEntity> Map(IUserSetCardOufEntity oufEntity);
}
