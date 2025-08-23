using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Apis;

public interface ICardAggregatorService
{
    Task<OperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args);
}
