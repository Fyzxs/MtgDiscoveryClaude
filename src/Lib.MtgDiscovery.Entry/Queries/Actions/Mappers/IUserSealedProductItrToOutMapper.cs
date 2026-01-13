using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserSealedProductItrToOutMapper : ICreateMapper<IUserSealedProductItrEntity, UserSealedProductOutEntity>;
