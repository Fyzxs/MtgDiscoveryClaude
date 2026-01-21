using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.SealedProducts;

public interface ISealedProductsBySetCodeEntryService : IOperationResponseService<ISealedProductsBySetCodeArgEntity, List<SealedProductOutEntity>>;
