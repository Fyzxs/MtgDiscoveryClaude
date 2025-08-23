using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.Aggregator.Cards.Apis;

public interface ICardAggregatorService
{
    Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args);
}
