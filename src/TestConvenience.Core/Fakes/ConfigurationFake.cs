using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace TestConvenience.Core.Fakes;

public sealed class ConfigurationFake : IConfiguration
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

    public IConfigurationSection GetSection(string key) => _configuration.GetSection(key);

    public IEnumerable<IConfigurationSection> GetChildren() => _configuration.GetChildren();

    public IChangeToken GetReloadToken() => _configuration.GetReloadToken();

    public string this[string key]
    {
        get => _configuration[key];
        set => _configuration[key] = value;
    }
}
