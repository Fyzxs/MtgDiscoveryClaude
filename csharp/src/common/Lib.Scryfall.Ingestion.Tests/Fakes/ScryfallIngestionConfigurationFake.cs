using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ScryfallIngestionConfigurationFake : IScryfallIngestionConfiguration
{
    private readonly IScryfallProcessingConfig _processingConfig;

    public ScryfallIngestionConfigurationFake(IScryfallProcessingConfig processingConfig = null)
    {
        _processingConfig = processingConfig ?? new ScryfallProcessingConfigFake();
    }

    public IScryfallProcessingConfig ProcessingConfig() => _processingConfig;
}
