using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class SetIdsArgEntity : ISetIdsArgEntity
{
    public ICollection<string> SetIds { get; init; }
    public string UserId { get; set; }
}
