namespace Lib.Scryfall.Ingestion.Configurations;

internal interface IScryfallProcessingConfig
{
    const string MaxSetsKey = "max_sets";
    const string SpecificSetsKey = "specific_sets";
    const string SetsReleasedAfterKey = "sets_released_after";
    const string SetBatchSizeKey = "batch_size";
    const string ProcessSetsInReverseKey = "reverse";
    const string AlwaysDownloadImagesKey = "always_download_images";
    const string ProcessOnlySetItemsKey = "process_only_set_items";
    MaxSetsToProcess MaxSets();
    SpecificSetCodes SpecificSets();
    ReleasedAfterDate SetsReleasedAfter();
    SetBatchSize SetBatchSize();
    ProcessSetsInReverse ProcessSetsInReverse();
    AlwaysDownloadImages AlwaysDownloadImages();
    ProcessOnlySetItems ProcessOnlySetItems();
}
