using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

// NOTE: Cannot extend ICreateMapper<dynamic, ICardItemItrEntity> due to C# language limitation:
// CS1966: Cannot implement a dynamic interface. Generic type parameters cannot be 'dynamic'.
public interface IDynamicToCardItemItrEntityMapper
{
    Task<ICardItemItrEntity> Map(dynamic source);
}
