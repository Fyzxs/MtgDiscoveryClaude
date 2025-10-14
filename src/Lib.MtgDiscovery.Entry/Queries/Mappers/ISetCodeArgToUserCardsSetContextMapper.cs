using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ISetCodeArgToUserCardsSetContextMapper : ICreateMapper<ISetCodeArgEntity, List<CardItemOutEntity>, IUserCardsSetItrEntity>
{
}
