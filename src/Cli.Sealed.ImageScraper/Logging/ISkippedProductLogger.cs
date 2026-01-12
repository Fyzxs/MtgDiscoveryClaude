using System;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.Logging;

internal interface ISkippedProductLogger : IDisposable
{
    void LogSkippedKnown(SealedProduct product, string reason);
    void LogSkippedNoImage(SealedProduct product);
    void LogSkippedUnknown(SealedProduct product, string reason);
}
