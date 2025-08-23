using System.Threading.Tasks;
using Lib.Shared.DataModels.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ICardEntryService
{
    Task<OperationStatus> CardsByIdsAsync(ICardIdsArgsEntity args);
}
