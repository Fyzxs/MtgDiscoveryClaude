using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Implementation of PartitionKeyValue that returns a PartitionKey from a provided string value.
/// </summary>
public sealed class ProvidedPartitionKeyValue : PartitionKeyValue
{
    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProvidedPartitionKeyValue"/> class.
    /// </summary>
    /// <param name="value">The partition key string value.</param>
    public ProvidedPartitionKeyValue(string value)
    {
        _value = value;
    }

    /// <summary>
    /// Returns a PartitionKey created from the provided string value.
    /// </summary>
    /// <returns>A PartitionKey instance created from the provided string.</returns>
    public override PartitionKey AsSystemType() => new(_value);
}
