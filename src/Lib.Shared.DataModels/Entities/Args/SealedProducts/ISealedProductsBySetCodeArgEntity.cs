using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.Shared.DataModels.Entities.Args.SealedProducts;

public interface ISealedProductsBySetCodeArgEntity : IUserIdArgEntity
{
    string SetCode { get; }
}
