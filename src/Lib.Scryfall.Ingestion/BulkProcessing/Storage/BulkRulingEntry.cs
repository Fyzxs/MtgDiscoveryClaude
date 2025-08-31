using System;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Storage;

internal sealed class BulkRulingEntry
{
    public string Source { get; init; }
    public DateTime PublishedAt { get; init; }
    public string Comment { get; init; }
}