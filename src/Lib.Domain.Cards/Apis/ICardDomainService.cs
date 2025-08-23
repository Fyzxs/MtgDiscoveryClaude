using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.Domain.Cards.Apis;

public interface ICardDomainService
{
    Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args);
}
