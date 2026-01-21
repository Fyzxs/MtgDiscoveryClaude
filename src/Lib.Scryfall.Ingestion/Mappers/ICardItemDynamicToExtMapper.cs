using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

namespace Lib.Scryfall.Ingestion.Mappers;

// NOTE: Cannot extend ICreateMapper<dynamic, ScryfallCardItemExtEntity> due to C# language limitation:
// CS1966: Cannot implement a dynamic interface. Generic type parameters cannot be 'dynamic'.
internal interface ICardItemDynamicToExtMapper
{
    Task<ScryfallCardItemExtEntity> Map(dynamic scryfallCard);
}
