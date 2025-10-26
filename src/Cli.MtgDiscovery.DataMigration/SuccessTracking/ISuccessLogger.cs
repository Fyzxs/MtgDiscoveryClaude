using System;
using System.Threading.Tasks;

namespace Cli.MtgDiscovery.DataMigration.SuccessTracking;

internal interface ISuccessLogger : IDisposable
{
    Task LogSuccessAsync(MigrationSuccess success);
    Task FlushAsync();
}
