using App.MtgDiscovery.GraphQL.Apis.Types;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Internal.Mappers;

internal interface IScryfallCardMapper
{
    ScryfallCardEntity Map(ScryfallCardItem entity);
}

internal sealed class ScryfallCardMapper : IScryfallCardMapper
{
    public ScryfallCardEntity Map(ScryfallCardItem entity)
    {
        return JsonConvert.DeserializeObject<ScryfallCardEntity>(JsonConvert.SerializeObject(entity.Data));
    }
}
