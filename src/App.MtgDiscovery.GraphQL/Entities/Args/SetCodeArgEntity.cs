using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class SetCodeArgEntity : ISetCodeArgEntity
{
    public string SetCode { get; set; }
}