namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// Defines the configuration for Shared Access Signature (SAS) authentication.
/// </summary>
public interface IBlobSasConfig
{
    /// <summary>
    /// Configuration key for the connection string.
    /// </summary>
    const string ConnectionStringKey = "connectionstring";

    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <returns>The connection string.</returns>
    BlobConnectionString ConnectionString();
}
