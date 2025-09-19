using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Outs.Sets;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface ICollectionSetItemOufToOutMapper : ICreateMapper<ISetItemCollectionOufEntity, List<ScryfallSetOutEntity>>;
