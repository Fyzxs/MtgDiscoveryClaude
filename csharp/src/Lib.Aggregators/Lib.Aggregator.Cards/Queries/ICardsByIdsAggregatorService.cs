using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Queries;

internal interface ICardsByIdsAggregatorService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ICardIdsItrEntity input,
        CancellationToken cancellationToken);
}
