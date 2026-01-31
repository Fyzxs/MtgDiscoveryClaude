using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISetCodeArgToUserCardsSetContextMapper : ICreateMapper<ISetCodeArgEntity, List<CardItemOutEntity>, IUserCardsSetItrEntity>
{
}
