using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration.Collections;

internal interface ICollectionMigrationOrchestrator
{
    Task<CollectionMigrationResult> ExecuteMigrationAsync();
}
