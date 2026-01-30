using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserSetCardOufToSetInformationMapper
{
    Task<SetInformationOutEntity> Map(IUserSetCardOufEntity oufEntity);
}
