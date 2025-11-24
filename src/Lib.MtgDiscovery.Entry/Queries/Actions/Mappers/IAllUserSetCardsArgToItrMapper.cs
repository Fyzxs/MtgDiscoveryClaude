using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IAllUserSetCardsArgToItrMapper
{
    Task<IAllUserSetCardsItrEntity> Map(IAllUserSetCardsArgEntity arg);
}