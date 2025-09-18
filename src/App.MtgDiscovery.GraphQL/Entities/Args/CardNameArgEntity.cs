using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class CardNameArgEntity : ICardNameArgEntity
{
    public string CardName { get; set; }
}
