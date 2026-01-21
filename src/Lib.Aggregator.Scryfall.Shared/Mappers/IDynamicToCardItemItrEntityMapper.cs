using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

public interface IDynamicToCardItemItrEntityMapper
{
    Task<ICardItemItrEntity> Map(dynamic source);
}
