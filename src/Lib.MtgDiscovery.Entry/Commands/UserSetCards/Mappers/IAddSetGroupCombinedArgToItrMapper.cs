using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal interface IAddSetGroupCombinedArgToItrMapper
{
    Task<IAddSetGroupToUserSetCardItrEntity> Map(IAddSetGroupToUserSetCardArgsEntity from);
}
