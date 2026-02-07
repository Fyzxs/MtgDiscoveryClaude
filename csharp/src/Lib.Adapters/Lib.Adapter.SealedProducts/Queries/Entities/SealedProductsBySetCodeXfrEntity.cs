namespace Lib.Adapter.SealedProducts.Queries.Entities;

internal sealed class SealedProductsBySetCodeXfrEntity : Apis.Entities.ISealedProductsBySetCodeXfrEntity
{
    public string SetCode { get; init; }
    public string CacheKey => $"sealed_products:set:{SetCode}";
}
