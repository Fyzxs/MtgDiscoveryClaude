namespace Lib.Cosmos.Apis.Ids;

/// <summary>
/// Implementation of CosmosItemId that returns a provided string value.
/// </summary>
public sealed class ProvidedCosmosItemId : CosmosItemId
{
    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProvidedCosmosItemId"/> class.
    /// </summary>
    /// <param name="value">The item ID value.</param>
    public ProvidedCosmosItemId(string value)
    {
        _value = value;
    }

    /// <summary>
    /// Returns the provided item ID value.
    /// </summary>
    /// <returns>The item ID value provided in the constructor.</returns>
    public override string AsSystemType() => _value;
}
