using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args.UserCards;

public sealed class UserCardsByIdsArgEntity : IUserCardsByIdsArgEntity
{
    public string UserId { get; init; }
    public ICollection<string> CardIds { get; init; }
}
