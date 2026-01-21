using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Mappers;

internal interface IFinishCountsXfrToExtMapper : ICreateMapper<IFinishCountsXfrEntity, FinishCountsExtEntity>
{
}

internal sealed class FinishCountsXfrToExtMapper : IFinishCountsXfrToExtMapper
{
    public Task<FinishCountsExtEntity> Map(IFinishCountsXfrEntity source)
    {
        return Task.FromResult(new FinishCountsExtEntity
        {
            Total = source.Total,
            NonFoil = source.NonFoil,
            Foil = source.Foil,
            Etched = source.Etched
        });
    }
}
