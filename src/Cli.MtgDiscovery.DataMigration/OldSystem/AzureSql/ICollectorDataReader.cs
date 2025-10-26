using System.Collections.Generic;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql;

internal interface ICollectorDataReader
{
    Task<IEnumerable<CollectorDataRecord>> ReadAllAsync(string collectorId);
    Task<int> GetTotalCountAsync(string collectorId);
}
