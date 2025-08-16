using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Lib.Universal.Primitives;

[DebuggerDisplay("[{GetType().Name}]:{AsSystemType()}")]
public abstract class ToSystemType<TSystemType>
{
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "AsSystemType method serves as the named alternate")]
    public static implicit operator TSystemType([NotNull] ToSystemType<TSystemType> source) => source.AsSystemType();

    public abstract TSystemType AsSystemType();

    public override string ToString() => $"{AsSystemType()}";
}

