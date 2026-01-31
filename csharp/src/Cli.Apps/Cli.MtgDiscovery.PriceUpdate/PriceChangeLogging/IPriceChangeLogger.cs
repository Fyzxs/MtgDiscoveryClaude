using System;

namespace Cli.MtgDiscovery.PriceUpdate.PriceChangeLogging;

internal interface IPriceChangeLogger : IDisposable
{
    void LogChange(PriceChangeRecord record);
}
