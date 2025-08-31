using System;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Models;

internal sealed class ScryfallBulkRuling : IScryfallBulkRuling
{
    [JsonProperty("object")]
    public string Object { get; init; }

    [JsonProperty("oracle_id")]
    public string OracleId { get; init; }

    [JsonProperty("source")]
    public string Source { get; init; }

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; init; }

    [JsonProperty("comment")]
    public string Comment { get; init; }
}