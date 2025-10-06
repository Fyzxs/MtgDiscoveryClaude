using System.Collections.Generic;

namespace Lib.Aggregator.UserSetCards.Entities;

public interface IUserSetCardFinishGroupOufEntity
{
    IReadOnlyCollection<string> Cards { get; }
}
