using System.Collections.Generic;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.OldSystem.AzureSql.Entities;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.Mapping;

internal sealed class OldSpecialMapper : IOldSpecialMapper
{
    private readonly ILogger _logger;

    public OldSpecialMapper(ILogger logger)
    {
        _logger = logger;
    }

    public Task<IEnumerable<(string special, int count)>> Map(CollectorDataRecord source)
    {
        List<(string special, int count)> results = new();

        if (source.Unmodified > 0)
        {
            results.Add(("none", source.Unmodified));
        }

        if (source.Signed > 0)
        {
            results.Add(("signed", source.Signed));
        }

        if (source.Proof > 0)
        {
            results.Add(("artist_proof", source.Proof));
        }

        if (source.Altered > 0)
        {
            results.Add(("altered", source.Altered));
        }

        return Task.FromResult<IEnumerable<(string special, int count)>>(results);
    }
}
