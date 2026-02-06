using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class MaxSetsToProcessFake : MaxSetsToProcess
{
    private readonly int _value;
    private readonly bool _isUnlimited;

    public MaxSetsToProcessFake(int value, bool isUnlimited = false)
    {
        _value = value;
        _isUnlimited = isUnlimited;
    }

    public override int AsSystemType() => _value;
    public override bool IsUnlimited() => _isUnlimited;
}
