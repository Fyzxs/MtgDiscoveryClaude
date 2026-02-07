using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Tests.Fakes;

public sealed class CardNameSearchAggregatorServiceFake : ICardNameSearchAggregatorService
{
    public IOperationResponse<ICardNameSearchCollectionOufEntity> ExecuteResult { get; init; } =
        new SuccessOperationResponse<ICardNameSearchCollectionOufEntity>(new CardNameSearchCollectionOufEntityFake());

    public int ExecuteInvokeCount { get; private set; }

    public Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> Execute(
        ICardSearchTermItrEntity input,
        CancellationToken cancellationToken)
    {
        ExecuteInvokeCount++;
        return Task.FromResult(ExecuteResult);
    }
}
