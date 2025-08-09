using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Lib.Universal.Configurations;

public sealed class MonoStateConfig : IConfig
{
    private static readonly Semaphore s_setOnce = new(1, 1);
    private static IConfigurationRoot s_configuration;

    public static void SetConfiguration(IConfigurationRoot configuration)
    {
        try
        {
            s_setOnce.WaitOne();
            HandleAlreadySet();
            s_configuration = configuration;
        }
        finally
        {
            s_setOnce.Release();
        }
    }

    private static void HandleAlreadySet()
    {
        if (s_configuration == null) return;
        throw new InvalidOperationException($"{nameof(MonoStateConfig)} already set.");
    }

    public string this[string key]
    {
        get => s_configuration?[key];
        set => s_configuration[key] = value;
    }
}
