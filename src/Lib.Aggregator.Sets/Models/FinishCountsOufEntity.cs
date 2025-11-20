using Lib.Shared.DataModels.Entities.Itrs;
using Newtonsoft.Json;

namespace Lib.Aggregator.Sets.Models;

internal sealed class FinishCountsOufEntity : IFinishCountsOufEntity
{
    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("nonFoil")]
    public int NonFoil { get; set; }

    [JsonProperty("foil")]
    public int Foil { get; set; }

    [JsonProperty("etched")]
    public int Etched { get; set; }
}
