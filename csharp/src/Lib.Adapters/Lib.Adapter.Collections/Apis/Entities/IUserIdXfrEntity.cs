using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.Collections.Apis.Entities;

public interface IUserIdXfrEntity : IXfrEntity
{
    string UserId { get; }
}
