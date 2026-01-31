using Lib.Shared.DataModels.Entities.Args.Cards;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

internal sealed class CardNameArgEntity : ICardNameArgEntity
{
    public string CardName { get; set; }
    public string UserId { get; set; }
}
