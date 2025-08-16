namespace Lib.Scryfall.Ingestion.Configurations;
internal interface IScryfallProcessingConfig
{
    const string MaxSetsKey = "max_sets";
    const string SpecificSetsKey = "specific_sets";
    MaxSetsToProcess MaxSets();
    SpecificSetCodes SpecificSets();
}
