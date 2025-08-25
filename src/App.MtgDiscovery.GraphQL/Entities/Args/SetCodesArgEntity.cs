using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;

namespace App.MtgDiscovery.GraphQL.Entities.Args;

public sealed class SetCodesArgEntity : ISetCodesArgEntity
{
    public ICollection<string> SetCodes { get; init; }
}