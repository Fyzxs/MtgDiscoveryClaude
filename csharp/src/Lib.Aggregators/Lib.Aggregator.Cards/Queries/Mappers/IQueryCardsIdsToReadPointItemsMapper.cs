using System.Collections.Generic;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface IQueryCardsIdsToReadPointItemsMapper : ICreateMapper<ICardIdsItrEntity, IEnumerable<ReadPointItem>>;
