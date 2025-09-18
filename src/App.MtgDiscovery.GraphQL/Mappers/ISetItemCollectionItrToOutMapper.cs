using System.Collections.Generic;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface ISetItemCollectionItrToOutMapper : ICreateMapper<IEnumerable<ISetItemItrEntity>, List<ScryfallSetOutEntity>>;
