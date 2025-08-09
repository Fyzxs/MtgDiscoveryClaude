namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// 
/// </summary>
public interface IBlobContainerDefinition
{
    /// <summary>
    /// Gets the name of the container.
    /// </summary>
    /// <returns>The container name.</returns>
    BlobFriendlyContainerName FriendlyContainerName();

    /// <summary>
    /// 
    /// </summary>
    BlobFriendlyAccountName FriendlyAccountName();
}
