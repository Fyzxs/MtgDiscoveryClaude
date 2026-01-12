using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ICollectionSealedProductOufToOutMapper : ICreateMapper<IEnumerable<ISealedProductOufEntity>, List<SealedProductOutEntity>>;
