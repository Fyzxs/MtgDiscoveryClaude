using Lib.Universal.Configurations;

namespace Lib.Scryfall.Ingestion.Configurations;

internal sealed class ConfigScryfallProcessingConfig : IScryfallProcessingConfig
{
    private readonly string _parentKey;
    private readonly IConfig _config;

    public ConfigScryfallProcessingConfig(string parentKey, IConfig config)
    {
        _parentKey = parentKey;
        _config = config;
    }

    public MaxSetsToProcess MaxSets()
    {
        return new ConfigMaxSetsToProcess($"{_parentKey}:{IScryfallProcessingConfig.MaxSetsKey}", _config);
    }

    public SpecificSetCodes SpecificSets()
    {
        return new ConfigSpecificSetCodes($"{_parentKey}:{IScryfallProcessingConfig.SpecificSetsKey}", _config);
    }

    public ReleasedAfterDate SetsReleasedAfter()
    {
        return new ConfigReleasedAfterDate($"{_parentKey}:{IScryfallProcessingConfig.SetsReleasedAfterKey}", _config);
    }

    public SetBatchSize SetBatchSize()
    {
        return new ConfigSetBatchSize($"{_parentKey}:{IScryfallProcessingConfig.SetBatchSizeKey}", _config);
    }

    public ProcessSetsInReverse ProcessSetsInReverse()
    {
        return new ConfigProcessSetsInReverse($"{_parentKey}:{IScryfallProcessingConfig.ProcessSetsInReverseKey}", _config);
    }

    public AlwaysDownloadImages AlwaysDownloadImages()
    {
        return new ConfigAlwaysDownloadImages($"{_parentKey}:{IScryfallProcessingConfig.AlwaysDownloadImagesKey}", _config);
    }

    public ProcessOnlySetItems ProcessOnlySetItems()
    {
        return new ConfigProcessOnlySetItems($"{_parentKey}:{IScryfallProcessingConfig.ProcessOnlySetItemsKey}", _config);
    }
}
