using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ICardEntryService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsArgEntity args);
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeArgEntity setCode);
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameArgEntity cardName);
    Task<IOperationResponse<ICardNameSearchResultCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm);
}
