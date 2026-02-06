namespace Lib.Universal.Primitives;

public sealed class Iso8601UtcNowString : TimeInstantString
{
    private readonly TimeInstantString _inner;

    public Iso8601UtcNowString()
        : this(new Iso8601TimeInstantString(new UtcNowTimeInstant()))
    { }

    private Iso8601UtcNowString(TimeInstantString inner) => _inner = inner;

    public override string AsSystemType() => _inner.AsSystemType();
}
