using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class SetCodeArgEntity : ISetCodeArgEntity
{
    public string SetCode { get; set; }
    public string UserId { get; set; }
}
