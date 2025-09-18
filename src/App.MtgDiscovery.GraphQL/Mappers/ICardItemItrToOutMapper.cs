using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface ICardItemItrToOutMapper : ICreateMapper<ICardItemItrEntity, CardItemOutEntity>
{
}