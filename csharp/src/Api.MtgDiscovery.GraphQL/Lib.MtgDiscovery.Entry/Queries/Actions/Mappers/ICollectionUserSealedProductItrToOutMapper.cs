using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface ICollectionUserSealedProductItrToOutMapper : ICreateMapper<IEnumerable<IUserSealedProductOufEntity>, List<UserSealedProductOutEntity>>;
