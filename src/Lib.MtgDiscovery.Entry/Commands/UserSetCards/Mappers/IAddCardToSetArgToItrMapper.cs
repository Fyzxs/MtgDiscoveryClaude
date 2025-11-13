using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal interface IAddCardToSetArgToItrMapper
{
    Task<IAddCardToSetItrEntity> Map(IAddCardToSetArgsEntity source);
}
