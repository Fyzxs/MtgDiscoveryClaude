using System.Collections.Generic;
using Lib.Shared.DataModels.Abstractions;

namespace Lib.Adapter.UserSetCards.Apis.Entities;

public interface IUserSetCardUpsertXfrEntity : IXfrEntity
{
    string UserId { get; }
    string SetId { get; }
    int TotalCards { get; }
    int UniqueCards { get; }
    ICollection<string> Collecting { get; }
    IDictionary<string, IUserSetCardGroupXfrEntity> Groups { get; }
}
