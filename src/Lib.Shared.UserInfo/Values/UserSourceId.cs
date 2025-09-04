using Lib.Universal.Primitives;

namespace Lib.Shared.UserInfo.Values;

public sealed class UserSourceId : StringEqualityToSystemType<UserSourceId>
{
    private readonly string _value;

    public UserSourceId(string value) => _value = value;

    public override string AsSystemType() => _value;
}