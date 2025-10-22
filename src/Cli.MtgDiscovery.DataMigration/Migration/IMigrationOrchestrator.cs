using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration.Migration;

public interface IMigrationOrchestrator
{
    Task<MigrationResult> ExecuteMigrationAsync();
}
