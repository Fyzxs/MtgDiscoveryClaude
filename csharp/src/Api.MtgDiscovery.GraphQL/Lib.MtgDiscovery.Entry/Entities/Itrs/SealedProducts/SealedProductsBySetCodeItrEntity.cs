using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Entities.Itrs.SealedProducts;

internal sealed class SealedProductsBySetCodeItrEntity : ISealedProductsBySetCodeItrEntity
{
    public string SetCode { get; init; }
}
