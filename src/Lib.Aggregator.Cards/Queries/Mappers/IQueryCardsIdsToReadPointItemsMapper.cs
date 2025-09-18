using System.Collections.Generic;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface IQueryCardsIdsToReadPointItemsMapper : ICreateMapper<ICardIdsItrEntity, IEnumerable<ReadPointItem>>;
