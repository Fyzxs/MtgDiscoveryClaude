using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

public interface IDynamicToCardItemItrEntityMapper
{
    Task<ICardItemItrEntity> Map(dynamic source);
}
