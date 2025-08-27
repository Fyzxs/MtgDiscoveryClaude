using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Cards.Apis;

public interface ICardDomainService
{
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode);
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName);
}
