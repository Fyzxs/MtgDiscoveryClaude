using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

public interface IUserSetCardFinishGroupOufEntity
{
    IReadOnlyCollection<string> Cards { get; }
}
