using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.SealedProducts;

public interface ISealedProductsBySetCodeArgEntity : IUserIdArgEntity, IOptionalCollectionIdArgModel
{
    string SetCode { get; }
}
