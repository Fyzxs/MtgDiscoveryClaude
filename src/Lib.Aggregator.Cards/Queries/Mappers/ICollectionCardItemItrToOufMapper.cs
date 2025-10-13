using System.Collections.Generic;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICollectionCardItemItrToOufMapper : ICreateMapper<IEnumerable<ICardItemItrEntity>, ICardItemCollectionOufEntity>
{
}
