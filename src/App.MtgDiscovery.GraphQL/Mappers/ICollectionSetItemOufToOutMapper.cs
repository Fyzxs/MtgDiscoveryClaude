using System.Collections.Generic;
using App.MtgDiscovery.GraphQL.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface ICollectionSetItemOufToOutMapper : ICreateMapper<IEnumerable<ISetItemItrEntity>, ICollection<ScryfallSetOutEntity>>;
