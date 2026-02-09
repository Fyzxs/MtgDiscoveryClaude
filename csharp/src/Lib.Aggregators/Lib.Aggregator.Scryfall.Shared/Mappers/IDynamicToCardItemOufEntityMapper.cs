using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

// NOTE: Cannot extend ICreateMapper<dynamic, ICardItemOufEntity> due to C# language limitation:
// CS1966: Cannot implement a dynamic interface. Generic type parameters cannot be 'dynamic'.
public interface IDynamicToCardItemOufEntityMapper
{
    Task<ICardItemOufEntity> Map(dynamic source);
}
