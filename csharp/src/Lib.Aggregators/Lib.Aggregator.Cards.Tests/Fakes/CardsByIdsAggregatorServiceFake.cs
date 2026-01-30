using System.Threading.Tasks;
using Lib.Aggregator.Cards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Tests.Fakes;

internal sealed class CardsByIdsAggregatorServiceFake : ICardsByIdsAggregatorService
{
    public IOperationResponse<ICardItemCollectionOufEntity> ExecuteResult { get; init; } =
        new SuccessOperationResponse<ICardItemCollectionOufEntity>(new CardItemCollectionOufEntityFake());

    public int ExecuteInvokeCount { get; private set; }

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(ICardIdsItrEntity input)
    {
        ExecuteInvokeCount++;
        return Task.FromResult(ExecuteResult);
    }
}
