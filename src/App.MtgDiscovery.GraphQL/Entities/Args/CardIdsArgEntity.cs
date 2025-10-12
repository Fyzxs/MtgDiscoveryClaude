using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class CardIdsArgEntity : ICardIdsArgEntity
{
    public ICollection<string> CardIds { get; set; }
    public string UserId { get; set; }
}
