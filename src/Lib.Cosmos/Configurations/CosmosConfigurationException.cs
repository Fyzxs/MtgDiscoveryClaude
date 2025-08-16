using System;

namespace Lib.Cosmos.Configurations;

/// <summary>
/// 
/// </summary>
public sealed class CosmosConfigurationException : Exception
{
    /// <inheritdoc />
    public CosmosConfigurationException() : base()
    { }
    /// <inheritdoc />
    public CosmosConfigurationException(string message) : base(message)
    { }
    /// <inheritdoc />
    public CosmosConfigurationException(string message, Exception innerException) : base(message, innerException)
    { }
}
