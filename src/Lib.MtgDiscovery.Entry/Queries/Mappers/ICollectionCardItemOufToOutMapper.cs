using System.Collections.Generic;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICollectionCardItemOufToOutMapper : ICreateMapper<ICardItemCollectionOufEntity, List<CardItemOutEntity>>;
