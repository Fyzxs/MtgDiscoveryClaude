using System;

namespace Lib.Scryfall.Ingestion.Http;

internal interface IRateLimitToken : IDisposable
{
    DateTime AcquiredAt { get; }
}
