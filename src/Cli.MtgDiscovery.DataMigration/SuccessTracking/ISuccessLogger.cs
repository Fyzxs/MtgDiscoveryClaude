using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration.SuccessTracking;

public interface ISuccessLogger
{
    Task LogSuccessAsync(MigrationSuccess success);
    Task FlushAsync();
}
