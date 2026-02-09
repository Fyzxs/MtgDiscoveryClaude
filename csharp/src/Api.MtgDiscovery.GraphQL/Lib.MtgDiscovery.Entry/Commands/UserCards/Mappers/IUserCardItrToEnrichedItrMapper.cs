using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards.Mappers;

internal interface IUserCardItrToEnrichedItrMapper
{
    Task<IUserCardItrEntity> Map(IUserCardItrEntity itrEntity, CardItemOutEntity cardItem, string cardNameGuid);
}
