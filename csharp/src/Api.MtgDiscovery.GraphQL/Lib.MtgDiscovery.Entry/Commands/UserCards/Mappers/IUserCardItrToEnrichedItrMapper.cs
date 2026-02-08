using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards.Mappers;

internal interface IUserCardItrToEnrichedItrMapper
{
    IUserCardItrEntity Map(IUserCardItrEntity itrEntity, CardItemOutEntity cardItem, string cardNameGuid);
}
