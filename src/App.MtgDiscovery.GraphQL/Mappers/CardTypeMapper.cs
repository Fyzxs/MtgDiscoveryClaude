using App.MtgDiscovery.GraphQL.Entities.Outs;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IScryfallCardMapper
{
    ScryfallCardOutEntity Map(ScryfallCardItem entity);
}

internal sealed class ScryfallCardMapper : IScryfallCardMapper
{
    public ScryfallCardOutEntity Map(ScryfallCardItem entity)
    {
        return JsonConvert.DeserializeObject<ScryfallCardOutEntity>(JsonConvert.SerializeObject(entity.Data));
    }
}
