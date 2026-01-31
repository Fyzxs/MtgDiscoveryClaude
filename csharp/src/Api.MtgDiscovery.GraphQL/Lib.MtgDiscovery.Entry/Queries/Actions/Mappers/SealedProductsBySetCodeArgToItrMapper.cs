using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Itrs.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Itrs.SealedProducts;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class SealedProductsBySetCodeArgToItrMapper : ISealedProductsBySetCodeArgToItrMapper
{
    public Task<ISealedProductsBySetCodeItrEntity> Map(ISealedProductsBySetCodeArgEntity source)
    {
        return Task.FromResult<ISealedProductsBySetCodeItrEntity>(new SealedProductsBySetCodeItrEntity
        {
            SetCode = source.SetCode
        });
    }
}
