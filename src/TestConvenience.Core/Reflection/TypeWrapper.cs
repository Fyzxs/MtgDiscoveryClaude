using Lib.Universal.Primitives;

namespace TestConvenience.Core.Reflection;

public abstract class TypeWrapper<T> : ToSystemType<T>
{
    private readonly T _type;

    protected TypeWrapper(params object[] args) : this((T)new PrivateCtor<T>(args))
    { }
    private TypeWrapper(T type) => _type = type;
    public override T AsSystemType() => _type;
}
