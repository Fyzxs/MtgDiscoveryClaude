using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration.Migration;

internal interface IMigrationOrchestrator
{
    Task<MigrationResult> ExecuteMigrationAsync();
}
