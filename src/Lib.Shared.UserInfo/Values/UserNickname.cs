using Lib.Universal.Primitives;

namespace Lib.Shared.UserInfo.Values;

public sealed class UserNickname : StringEqualityToSystemType<UserNickname>
{
    private readonly string _value;

    public UserNickname(string value) => _value = value;

    public override string AsSystemType() => _value;
}