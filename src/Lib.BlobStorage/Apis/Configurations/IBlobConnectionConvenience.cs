using Azure.Core;

namespace Lib.BlobStorage.Apis.Configurations;

/// <summary>
/// Provides convenience methods for accessing Blob DB configuration and credentials
/// for a given container definition.
/// </summary>
public interface IBlobConnectionConvenience
{
    /// <summary>
    /// Gets the Blob DB account configuration for the specified container definition.
    /// </summary>
    /// <param name="blobContainerDefinition">The container definition.</param>
    /// <returns>The account configuration.</returns>
    IBlobAccountConfig AccountConfig(IBlobContainerDefinition blobContainerDefinition);

    /// <summary>
    /// Gets the Azure Entra (Active Directory) credential for the specified container definition.
    /// </summary>
    /// <param name="blobContainerDefinition">The container definition.</param>
    /// <returns>The token credential.</returns>
    TokenCredential AccountEntraCredential(IBlobContainerDefinition blobContainerDefinition);

    /// <summary>
    /// Gets the container configuration for the specified container definition.
    /// </summary>
    /// <param name="blobContainerDefinition">The container definition.</param>
    /// <returns>The container configuration.</returns>
    IBlobContainerConfig ContainerConfig(IBlobContainerDefinition blobContainerDefinition);
}
