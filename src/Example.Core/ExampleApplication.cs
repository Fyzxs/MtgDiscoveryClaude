using System.Threading.Tasks;
using Lib.Universal.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Example.Core;

public abstract class ExampleApplication
{
    private static readonly ISimpleLogger s_log = new SimpleConsoleLogger();
    protected void Log(string msg) => s_log.Log(msg);

    public async Task StartUp(string[] args)
    {
        Log($"Starting up {GetType().Name}...");
        LoadConfiguration(args);
        await Execute().ConfigureAwait(false);
    }

    protected virtual Task Execute() => Task.CompletedTask;

    private void LoadConfiguration(string[] args)
    {
        // Simulate loading configuration
        Log("Loading configuration...");
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json", optional: true);
                builder.AddJsonFile("local.settings.json", optional: true);
            })
            .ConfigureAppConfiguration(LoadApplicationConfiguration)
            .ConfigureServices((hostContext, _) =>
            {
                Log("Setting MonoState Config, with a risky hard cast");
                MonoStateConfig.SetConfiguration((IConfigurationRoot)hostContext.Configuration);
            })
            .Build();
    }

    protected virtual void LoadApplicationConfiguration(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder) { }
}
