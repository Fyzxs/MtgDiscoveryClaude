using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Operations;

namespace Lib.MtgDiscovery.Data.Apis;

public interface ICardDataService
{
    Task<OperationStatus> CardsByIdsAsync(ICardIdsItrEntity args);
}
