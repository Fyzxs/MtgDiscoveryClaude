using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;

internal interface IUserSealedProductIntegrator
{
    Task<List<SealedProductOutEntity>> Integrate(
        List<SealedProductOutEntity> current,
        List<UserSealedProductOutEntity> change);
}
