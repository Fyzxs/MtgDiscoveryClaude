using System;

namespace Lib.Scryfall.Ingestion.Configurations;

/// <summary>
/// Exception thrown when there is a configuration error for Scryfall settings.
/// </summary>
public sealed class ScryfallConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScryfallConfigurationException"/> class.
    /// </summary>
    public ScryfallConfigurationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScryfallConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ScryfallConfigurationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScryfallConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ScryfallConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
