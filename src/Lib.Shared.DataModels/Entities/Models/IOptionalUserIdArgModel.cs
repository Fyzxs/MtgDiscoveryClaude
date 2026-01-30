namespace Lib.Shared.DataModels.Entities.Models;

public interface IOptionalUserIdArgModel : IUserIdArgModel
{
    bool HasUserId => string.IsNullOrEmpty(UserId) is false;
}
