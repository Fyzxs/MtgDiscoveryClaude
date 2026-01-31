using Lib.Shared.DataModels.Entities.Args.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

internal sealed class CardSearchTermArgEntity : ICardSearchTermArgEntity
{
    public string SearchTerm { get; set; }
}
