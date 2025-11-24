using Lib.Shared.DataModels.Entities.Args.User;

namespace Lib.Shared.DataModels.Entities.Args.Sets;

public interface ISetCodeArgEntity : IUserIdArgEntity
{
    string SetCode { get; }
}
