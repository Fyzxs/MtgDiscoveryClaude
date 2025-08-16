using System;

namespace Lib.Scryfall.Ingestion.Internal.Http;
internal interface IRateLimitToken : IDisposable
{
    DateTime AcquiredAt { get; }
}
