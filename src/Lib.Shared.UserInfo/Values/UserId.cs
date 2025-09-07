using Lib.Universal.Primitives;

namespace Lib.Shared.UserInfo.Values;

//TODO: There are no other Lib.Shared.{DOMAIN} projects; why have this one?
public sealed class UserId : StringEqualityToSystemType<UserId>
{
    private readonly string _value;

    public UserId(string value) => _value = value;

    public override string AsSystemType() => _value;
}
