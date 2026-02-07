using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Commands.UserSealedProducts;

internal interface IAddUserSealedProductEntryService
    : IOperationResponseService<IAddSealedProductToCollectionArgsEntity, List<SealedProductOutEntity>>;
