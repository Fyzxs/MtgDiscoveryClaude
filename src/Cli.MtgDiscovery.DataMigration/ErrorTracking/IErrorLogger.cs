using System;
using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration.ErrorTracking;

internal interface IErrorLogger : IDisposable
{
    Task LogErrorAsync(MigrationError error);
    Task FlushAsync();
}
