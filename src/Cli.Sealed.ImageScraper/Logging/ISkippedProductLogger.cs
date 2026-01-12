using System;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.Logging;

internal interface ISkippedProductLogger : IDisposable
{
    void LogSkipped(SealedProduct product, string reason);
}
