using Lib.BlobStorage.Apis.Configurations.Ids;

namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// Defines the optional configuration settings for an Azure Blob DB container.
/// </summary>
public interface IBlobContainerConfig
{
    /// <summary>
    /// 
    /// </summary>
    const string ContainerNameKey = "container_name";
    /// <summary>
    /// 
    /// </summary>
    const string PublicAccessKey = "public_access";

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    BlobContainerName Name();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    BlobContainerPublicAccessType AccessType();
}
