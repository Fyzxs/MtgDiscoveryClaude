using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface ISetItemItrToOutMapper : ICreateMapper<ISetItemItrEntity, ScryfallSetOutEntity>
{
}