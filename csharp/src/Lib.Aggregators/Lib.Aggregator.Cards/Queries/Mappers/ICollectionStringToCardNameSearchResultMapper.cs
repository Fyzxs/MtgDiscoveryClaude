using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICollectionStringToCardNameSearchResultMapper : ICreateMapper<IEnumerable<string>, ICollection<ICardNameSearchResultItrEntity>>
{
}
