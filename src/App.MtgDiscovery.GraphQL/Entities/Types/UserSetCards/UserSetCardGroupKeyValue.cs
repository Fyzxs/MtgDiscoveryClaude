using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Entities.Types.UserSetCards;

public sealed class UserSetCardGroupKeyValue
{
    public string Key { get; init; }
    public UserSetCardGroupOutEntity Value { get; init; }
}