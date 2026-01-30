using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserCards;

public interface IUserCardsBySetArgEntity : IUserIdArgModel
{
    string SetId { get; }
}
