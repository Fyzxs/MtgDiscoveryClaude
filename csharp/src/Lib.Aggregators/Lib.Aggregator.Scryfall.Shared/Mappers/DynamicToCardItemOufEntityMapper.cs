using System.Threading.Tasks;
using Lib.Aggregator.Scryfall.Shared.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Scryfall.Shared.Mappers;

public sealed class DynamicToCardItemOufEntityMapper : IDynamicToCardItemOufEntityMapper
{
    public Task<ICardItemOufEntity> Map(dynamic source) => Task.FromResult<ICardItemOufEntity>(new CardItemOufEntity(source));
}
