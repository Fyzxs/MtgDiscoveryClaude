using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Integrators;

internal sealed class UserSealedProductIntegrator : IUserSealedProductIntegrator
{
    public Task<List<SealedProductOutEntity>> Integrate(
        List<SealedProductOutEntity> current,
        List<UserSealedProductOutEntity> change)
    {
        Dictionary<string, int> userQuantities = change.ToDictionary(
            up => up.ProductUuid,
            up => up.Count
        );

        foreach (SealedProductOutEntity product in current)
        {
            if (userQuantities.TryGetValue(product.Uuid, out int quantity))
            {
                product.UserQuantity = quantity;
            }
        }

        return Task.FromResult(current);
    }
}
