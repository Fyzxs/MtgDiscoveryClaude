using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class CardNameArgEntity : ICardNameArgEntity
{
    public string CardName { get; set; }
}
