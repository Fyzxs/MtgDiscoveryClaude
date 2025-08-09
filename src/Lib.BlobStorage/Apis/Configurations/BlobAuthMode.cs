namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// Specifies the authentication mode for connecting to Azure Blob DB.
/// </summary>
public enum BlobAuthMode
{
    /// <summary>
    /// Key-based authentication using account keys.
    /// </summary>
    KeyAuth = 0,

    /// <summary>
    /// Azure Active Directory (AAD) authentication.
    /// </summary>
    EntraAuth = 1
}
