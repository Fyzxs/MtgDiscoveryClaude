using Lib.Shared.DataModels.Abstractions;
using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserCards;

public interface IUserCardArgEntity : IArgEntity, IUserIdArgModel
{
    string CardId { get; }
}
