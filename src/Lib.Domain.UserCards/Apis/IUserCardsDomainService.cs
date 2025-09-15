using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.UserCards.Apis;

public interface IUserCardsDomainService
{
    Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard);
}
