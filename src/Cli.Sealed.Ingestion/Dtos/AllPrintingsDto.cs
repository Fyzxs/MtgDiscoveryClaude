using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cli.Sealed.Ingestion.Dtos;

internal sealed class AllPrintingsDto
{
    [JsonProperty("data")]
    public Dictionary<string, MtgJsonSetDto> Data { get; init; }
}
