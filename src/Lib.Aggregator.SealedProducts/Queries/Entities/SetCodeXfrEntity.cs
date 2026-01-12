using Lib.Adapter.SealedProducts.Apis.Entities;

namespace Lib.Aggregator.SealedProducts.Queries.Entities;

internal sealed class SetCodeXfrEntity : ISealedProductsBySetCodeXfrEntity
{
    public string SetCode { get; init; }
}
