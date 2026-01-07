using System.Threading.Tasks;

namespace Cli.MtgDiscovery.PriceUpdate.Orchestration;

internal interface IPriceUpdateOrchestrator
{
    Task<PriceUpdateResult> ExecuteAsync();
}
