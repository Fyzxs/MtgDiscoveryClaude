using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration.ErrorTracking;

public interface IErrorLogger
{
    Task LogErrorAsync(MigrationError error);
    Task FlushAsync();
}
