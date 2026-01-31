using System;

namespace Cli.MtgDiscovery.PriceUpdate.ErrorTracking;

internal interface IPriceUpdateErrorLogger : IDisposable
{
    void LogError(PriceUpdateError error);
}
