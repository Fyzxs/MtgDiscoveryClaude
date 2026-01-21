using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cli.Sealed.ImageScraper.MtgJson.Dtos;

internal sealed class MtgJsonSetDto
{
    [JsonProperty("code")]
    public string Code { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("sealedProduct")]
    public List<MtgJsonSealedProductDto> SealedProduct { get; init; }
}
