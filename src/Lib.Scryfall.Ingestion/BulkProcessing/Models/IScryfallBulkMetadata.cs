using System;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Models;

internal interface IScryfallBulkMetadata
{
    string Object { get; }
    string Id { get; }
    string Type { get; }
    DateTime UpdatedAt { get; }
    string Uri { get; }
    string Name { get; }
    string Description { get; }
    long Size { get; }
    string DownloadUri { get; }
    string ContentType { get; }
    string ContentEncoding { get; }
}