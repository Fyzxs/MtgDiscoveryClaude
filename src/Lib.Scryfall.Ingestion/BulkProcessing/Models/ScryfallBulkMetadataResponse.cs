using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Models;

internal sealed class ScryfallBulkMetadataResponse : IScryfallBulkMetadataResponse
{
    [JsonProperty("object")]
    public string Object { get; init; }

    [JsonProperty("has_more")]
    public bool HasMore { get; init; }

    [JsonProperty("data")]
    public IEnumerable<IScryfallBulkMetadata> Data { get; init; }
}