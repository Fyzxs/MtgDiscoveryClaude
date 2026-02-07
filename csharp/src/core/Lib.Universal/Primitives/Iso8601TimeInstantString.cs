namespace Lib.Universal.Primitives;

public sealed class Iso8601TimeInstantString : TimeInstantString
{
    private readonly TimeInstant _timeInstant;

    public Iso8601TimeInstantString(TimeInstant timeInstant) => _timeInstant = timeInstant;

    public override string AsSystemType() => _timeInstant.AsSystemType().ToString("o");
}
