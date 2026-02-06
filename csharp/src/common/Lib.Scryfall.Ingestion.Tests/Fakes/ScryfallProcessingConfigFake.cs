using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ScryfallProcessingConfigFake : IScryfallProcessingConfig
{
    private readonly MaxSetsToProcess _maxSets;
    private readonly SpecificSetCodes _specificSets;
    private readonly ReleasedAfterDate _releasedAfter;

    public ScryfallProcessingConfigFake(
        MaxSetsToProcess maxSets = null,
        SpecificSetCodes specificSets = null,
        ReleasedAfterDate releasedAfter = null)
    {
        _maxSets = maxSets ?? new MaxSetsToProcessFake(0, isUnlimited: true);
        _specificSets = specificSets ?? new SpecificSetCodesFake();
        _releasedAfter = releasedAfter ?? new ReleasedAfterDateFake();
    }

    public MaxSetsToProcess MaxSets() => _maxSets;
    public SpecificSetCodes SpecificSets() => _specificSets;
    public ReleasedAfterDate SetsReleasedAfter() => _releasedAfter;
    public SetBatchSize SetBatchSize() => throw new System.NotImplementedException();
    public ProcessSetsInReverse ProcessSetsInReverse() => throw new System.NotImplementedException();
    public AlwaysDownloadImages AlwaysDownloadImages() => throw new System.NotImplementedException();
    public ProcessOnlySetItems ProcessOnlySetItems() => throw new System.NotImplementedException();
}
