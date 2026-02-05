using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserCards.Queries.UserCardsByArtist;

internal interface IUserCardsByArtistAggregatorService
{
    Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> Execute(
        IUserCardsArtistItrEntity input,
        CancellationToken cancellationToken);
}
