using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICollectionCardItemOufToOutMapper : ICreateMapper<ICardItemCollectionOufEntity, List<CardItemOutEntity>>;
