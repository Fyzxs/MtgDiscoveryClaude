using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        DataMigrationApplication application = new DataMigrationApplication(configuration, loggerFactory);

        try
        {
            int result = await application.RunAsync().ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Unhandled exception in migration application");
            return 1;
        }
    }
}
