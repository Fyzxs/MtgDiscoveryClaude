using Lib.Shared.DataModels.Entities.Args.SealedProducts;

namespace App.MtgDiscovery.GraphQL.Entities.Args.SealedProducts;

public sealed class GetSealedProductsBySetCodeArgEntity : ISealedProductsBySetCodeArgEntity
{
    public string SetCode { get; init; }
    public string UserId { get; init; }
}
