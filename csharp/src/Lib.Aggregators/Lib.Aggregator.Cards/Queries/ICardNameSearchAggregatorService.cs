using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Queries;

internal interface ICardNameSearchAggregatorService
{
    Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> Execute(
        ICardSearchTermItrEntity input,
        CancellationToken cancellationToken);
}
