using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ICollectionUserSetCardOufToOutMapper
{
    Task<List<UserSetCardOutEntity>> Map(IEnumerable<IUserSetCardOufEntity> userSetCards);
}