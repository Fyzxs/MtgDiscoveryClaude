using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.SealedProducts.Apis.Entities;

public interface ISealedProductsBySetCodeXfrEntity : IXfrEntity
{
    string SetCode { get; }
}
