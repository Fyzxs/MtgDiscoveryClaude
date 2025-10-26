namespace Cli.MtgDiscovery.DataMigration.Migration;

internal interface IMigrationProgressTracker
{
    void Initialize(int totalRecords);
    void IncrementProgress();
    void Complete();
}
