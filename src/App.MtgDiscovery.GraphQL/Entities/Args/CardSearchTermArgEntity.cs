using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class CardSearchTermArgEntity : ICardSearchTermArgEntity
{
    public string SearchTerm { get; set; }
}
