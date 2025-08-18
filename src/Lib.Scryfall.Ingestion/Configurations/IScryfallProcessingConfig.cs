namespace Lib.Scryfall.Ingestion.Configurations;
internal interface IScryfallProcessingConfig
{
    const string MaxSetsKey = "max_sets";
    const string SpecificSetsKey = "specific_sets";
    const string SetBatchSizeKey = "batch_size";
    const string ProcessSetsInReverseKey = "reverse";
    const string AlwaysDownloadImagesKey = "always_download_images";
    MaxSetsToProcess MaxSets();
    SpecificSetCodes SpecificSets();
    SetBatchSize SetBatchSize();
    ProcessSetsInReverse ProcessSetsInReverse();
    AlwaysDownloadImages AlwaysDownloadImages();
}
