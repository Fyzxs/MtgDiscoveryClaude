using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.Mapping;

internal sealed class OldFinishMapper : IOldFinishMapper
{
    private readonly ILogger _logger;

    public OldFinishMapper(ILogger logger)
    {
        _logger = logger;
    }

    public Task<string> Map((bool foil, bool nonfoil, bool etched) source)
    {
        int trueCount = 0;
        if (source.foil) trueCount++;
        if (source.nonfoil) trueCount++;
        if (source.etched) trueCount++;

        if (trueCount != 1)
        {
            string message = $"Exactly one finish flag must be true. Found {trueCount} true values (foil:{source.foil}, nonfoil:{source.nonfoil}, etched:{source.etched})";
            _logger.LogError(message);
            throw new InvalidOperationException(message);
        }

        string finish = source.foil ? "foil" : source.nonfoil ? "nonfoil" : "etched";

        return Task.FromResult(finish);
    }
}
