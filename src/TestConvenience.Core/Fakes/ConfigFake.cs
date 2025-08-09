using System.Collections.Generic;
using Lib.Universal.Configurations;

namespace TestConvenience.Core.Fakes;

public class ConfigFake : IConfig
{
    private readonly Dictionary<string, string> _values = [];

    public string this[string key]
    {
        get => _values.TryGetValue(key, out string value) ? value : string.Empty;
        set => _values[key] = value;
    }
}
