using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

public interface IUserSetCardFinishGroupOufEntity
{
    IReadOnlyCollection<string> Cards { get; }
}
