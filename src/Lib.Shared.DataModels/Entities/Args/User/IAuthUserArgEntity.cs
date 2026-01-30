using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.User;

public interface IAuthUserArgEntity : IUserIdArgModel
{
    string SourceId { get; }
    string DisplayName { get; }
    string Email { get; }
}
