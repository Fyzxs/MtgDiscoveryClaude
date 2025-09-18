using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Apis;

public interface ICardAggregatorService
{
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName);
    Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm);
}
