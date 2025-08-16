using Lib.Scryfall.Ingestion.Configurations;

namespace Lib.Scryfall.Ingestion.Apis.Configurations;

/// <summary>
/// Provides processing configuration for Scryfall ingestion.
/// </summary>
public interface IScryfallProcessingConfig
{
    /// <summary>
    /// 
    /// </summary>
    const string MaxSetsKey = "max_sets";

    /// <summary>
    /// 
    /// </summary>
    const string SpecificSetsKey = "specific_sets";

    /// <summary>
    /// Gets the maximum number of sets to process.
    /// </summary>
    MaxSetsToProcess MaxSets();

    /// <summary>
    /// Gets the specific set codes to process.
    /// </summary>
    SpecificSetCodes SpecificSets();
}
