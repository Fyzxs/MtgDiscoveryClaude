using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class CardSearchTermArgEntity : ICardSearchTermArgEntity
{
    public string SearchTerm { get; set; }
}
