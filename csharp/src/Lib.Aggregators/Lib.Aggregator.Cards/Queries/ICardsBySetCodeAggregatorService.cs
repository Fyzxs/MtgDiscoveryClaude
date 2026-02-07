using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Queries;

internal interface ICardsBySetCodeAggregatorService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ISetCodeItrEntity input,
        CancellationToken cancellationToken);
}
