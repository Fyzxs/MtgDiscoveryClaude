using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface ICollectionCardItemItrToOufMapper : ICreateMapper<IEnumerable<ICardItemItrEntity>, ICardItemCollectionOufEntity>
{
}
