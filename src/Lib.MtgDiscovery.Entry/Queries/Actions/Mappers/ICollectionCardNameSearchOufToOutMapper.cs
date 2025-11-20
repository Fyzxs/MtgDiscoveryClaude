using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ICollectionCardNameSearchOufToOutMapper : ICreateMapper<ICardNameSearchCollectionOufEntity, List<CardNameSearchResultOutEntity>>
{
}
