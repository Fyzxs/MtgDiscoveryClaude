using System;

namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// 
/// </summary>
public sealed class BlobConfigurationException : Exception
{
    /// <inheritdoc />
    public BlobConfigurationException() : base()
    { }
    /// <inheritdoc />
    public BlobConfigurationException(string message) : base(message)
    { }
    /// <inheritdoc />
    public BlobConfigurationException(string message, Exception innerException) : base(message, innerException)
    { }
}
