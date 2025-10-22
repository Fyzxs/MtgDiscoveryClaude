namespace Cli.MtgDiscovery.DataMigration.Migration;

public interface IMigrationProgressTracker
{
    void Initialize(int totalRecords);
    void IncrementProgress();
    void Complete();
}
