using System;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.BulkProcessing.Models;

internal sealed class ScryfallBulkMetadata : IScryfallBulkMetadata
{
    [JsonProperty("object")]
    public string Object { get; init; }

    [JsonProperty("id")]
    public string Id { get; init; }

    [JsonProperty("type")]
    public string Type { get; init; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; init; }

    [JsonProperty("uri")]
    public string Uri { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("description")]
    public string Description { get; init; }

    [JsonProperty("size")]
    public long Size { get; init; }

    [JsonProperty("download_uri")]
    public string DownloadUri { get; init; }

    [JsonProperty("content_type")]
    public string ContentType { get; init; }

    [JsonProperty("content_encoding")]
    public string ContentEncoding { get; init; }
}