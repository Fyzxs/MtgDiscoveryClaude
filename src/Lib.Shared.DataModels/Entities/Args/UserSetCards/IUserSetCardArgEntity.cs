using Lib.Shared.DataModels.Abstractions;
using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserSetCards;

public interface IUserSetCardArgEntity : IArgEntity, IUserIdArgModel
{
    string SetId { get; }
}
