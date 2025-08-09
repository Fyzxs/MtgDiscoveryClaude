using Azure.Core;
using Lib.BlobStorage.Apis.Configurations;
using Lib.Universal.Inversion;

namespace Lib.BlobStorage.Apis;

/// <inheritdoc />
public sealed class ServiceLocatorAuthBlobConnectionConfig : IBlobConnectionConvenience
{
    private readonly IBlobConfiguration _config;

    private readonly IServiceLocator _serviceLocator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceLocatorAuthBlobConnectionConfig"/> class
    /// using the default configuration and service locator.
    /// </summary>
    public ServiceLocatorAuthBlobConnectionConfig() : this(new ConfigBlobConfiguration(), ServiceLocator.Instance)
    { }

    private ServiceLocatorAuthBlobConnectionConfig(IBlobConfiguration config, IServiceLocator serviceLocator)
    {
        _config = config;
        _serviceLocator = serviceLocator;
    }

    /// <inheritdoc />
    public IBlobAccountConfig AccountConfig(IBlobContainerDefinition blobContainerDefinition) => _config.AccountConfig(blobContainerDefinition);

    /// <inheritdoc />
    public TokenCredential AccountEntraCredential(IBlobContainerDefinition blobContainerDefinition) => _serviceLocator.GetService<TokenCredential, TokenCredential>();

    /// <inheritdoc />
    public IBlobContainerConfig ContainerConfig(IBlobContainerDefinition blobContainerDefinition) => _config.AccountConfig(blobContainerDefinition).ContainerConfig(blobContainerDefinition);
}
