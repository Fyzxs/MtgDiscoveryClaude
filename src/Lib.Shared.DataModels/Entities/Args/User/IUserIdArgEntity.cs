using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.User;

public interface IUserIdArgEntity : IOptionalUserIdArgModel
{
    bool DoesNotHaveUserId => HasUserId is false;
}
