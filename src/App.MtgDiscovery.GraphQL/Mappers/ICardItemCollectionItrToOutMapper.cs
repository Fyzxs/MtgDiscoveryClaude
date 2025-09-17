using System.Collections.Generic;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface ICardItemCollectionItrToOutMapper : ICreateMapper<IEnumerable<ICardItemItrEntity>, List<CardItemOutEntity>>;
