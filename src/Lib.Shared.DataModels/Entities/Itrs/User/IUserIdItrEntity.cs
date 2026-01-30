using Lib.Shared.DataModels.Abstractions;

namespace Lib.Shared.DataModels.Entities.Itrs.User;

public interface IUserIdItrEntity : IItrEntity
{
    string UserId { get; }
}
