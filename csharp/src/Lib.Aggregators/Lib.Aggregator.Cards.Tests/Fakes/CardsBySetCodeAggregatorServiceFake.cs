using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CardsBySetCodeAggregatorServiceFake : ICardsBySetCodeAggregatorService
{
    public IOperationResponse<ICardItemCollectionOufEntity> ExecuteResult { get; init; } =
        new SuccessOperationResponse<ICardItemCollectionOufEntity>(new CardItemCollectionOufEntityFake());

    public int ExecuteInvokeCount { get; private set; }

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ISetCodeItrEntity input,
        CancellationToken cancellationToken)
    {
        ExecuteInvokeCount++;
        return Task.FromResult(ExecuteResult);
    }
}
