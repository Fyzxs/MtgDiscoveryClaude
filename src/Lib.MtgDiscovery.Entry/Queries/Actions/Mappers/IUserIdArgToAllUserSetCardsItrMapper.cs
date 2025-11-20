using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserIdArgToAllUserSetCardsItrMapper
{
    Task<IAllUserSetCardsItrEntity> Map(IUserIdArgEntity arg);
}
