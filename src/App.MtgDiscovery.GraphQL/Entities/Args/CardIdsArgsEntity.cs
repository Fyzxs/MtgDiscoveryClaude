using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class CardIdsArgsEntity : ICardIdsArgsEntity
{
    public ICollection<string> CardIds { get; set; }
}
