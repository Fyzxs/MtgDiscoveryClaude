using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ISealedProductsBySetCodeArgToItrMapper : ICreateMapper<ISealedProductsBySetCodeArgEntity, ISealedProductsBySetCodeItrEntity>;
