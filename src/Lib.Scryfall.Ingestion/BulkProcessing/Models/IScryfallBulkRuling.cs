using System;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Models;

internal interface IScryfallBulkRuling
{
    string Object { get; }
    string OracleId { get; }
    string Source { get; }
    DateTime PublishedAt { get; }
    string Comment { get; }
}