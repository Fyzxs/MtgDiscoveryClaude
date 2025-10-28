using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Queries.UserCardsByArtist;

internal interface IUserCardsByArtistAggregatorService
    : IOperationResponseService<IUserCardsArtistItrEntity, IEnumerable<IUserCardOufEntity>>;
