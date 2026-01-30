using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Models;

namespace Lib.Shared.DataModels.Entities.Args.UserCards;

public interface IUserCardsByIdsArgEntity : IUserIdArgModel
{
    ICollection<string> CardIds { get; }
}
