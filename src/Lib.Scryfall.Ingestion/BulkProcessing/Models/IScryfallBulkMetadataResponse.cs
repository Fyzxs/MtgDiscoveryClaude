using System.Collections.Generic;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Models;

internal interface IScryfallBulkMetadataResponse
{
    string Object { get; }
    bool HasMore { get; }
    IEnumerable<IScryfallBulkMetadata> Data { get; }
}