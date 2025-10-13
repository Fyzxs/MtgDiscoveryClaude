using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

internal sealed class SetCardItemDynamicToExtMapper : ISetCardItemDynamicToExtMapper
{
    public Task<ScryfallSetCardItemExtEntity> Map(dynamic scryfallCard)
    {
        ScryfallSetCardItemExtEntity result = new() { Data = scryfallCard };
        return Task.FromResult(result);
    }
}
