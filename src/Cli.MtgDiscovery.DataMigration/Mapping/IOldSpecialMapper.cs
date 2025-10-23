using System.Collections.Generic;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;
using Lib.Shared.Abstractions.Mappers;

namespace Cli.MtgDiscovery.DataMigration.Mapping;

public interface IOldSpecialMapper : ICreateMapper<CollectorDataRecord, IEnumerable<(string special, int count)>>
{
}
