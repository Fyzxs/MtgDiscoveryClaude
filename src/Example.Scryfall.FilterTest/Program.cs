using System;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Collections;
using Lib.Scryfall.Shared.Apis.Models;
using Lib.Universal.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Example.Scryfall.FilterTest;

internal class Program
{
    static async Task Main()
    {
        // Initialize configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        MonoStateConfig.SetConfiguration(configuration);

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();

        logger.LogInformation("Testing Scryfall Set Filters");
        logger.LogInformation("=============================");
        logger.LogInformation("Filters applied:");
        logger.LogInformation("  - NonDigitalSetFilter (excluding digital-only sets)");
        logger.LogInformation("  - PreviewSetFilter (excluding unreleased sets)");
        logger.LogInformation("  - ForeignSetFilter (excluding foreign language sets: 4BB, BCHR, REN, RIN, FBB)");

        FilteredScryfallSetCollection filteredSets = new(logger);

        int totalSets = 0;

        logger.LogInformation("\nProcessing sets (first 10 included sets):");
        logger.LogInformation("------------------------------------------");

        await foreach (IScryfallSet set in filteredSets.ConfigureAwait(false))
        {
            totalSets++;

            if (totalSets <= 10)
            {
                dynamic data = set.Data();
                string releasedAt = data.released_at ?? "N/A";
                logger.LogInformation("✓ {Code} - {Name} (Released: {ReleaseDate})",
                    set.Code(),
                    set.Name(),
                    releasedAt);
            }

            if (totalSets >= 10)
            {
                break;
            }
        }

        logger.LogInformation("\nFilter Summary:");
        logger.LogInformation("---------------");
        logger.LogInformation("Processed {Count} sets that passed all filters", totalSets);
        logger.LogInformation("\nNote: To see excluded sets, check the logs above");
        logger.LogInformation("Foreign sets that would be excluded: 4BB, BCHR, REN, RIN, FBB");
        logger.LogInformation("Digital sets are automatically excluded");
        logger.LogInformation("Preview/future sets are automatically excluded");

        logger.LogInformation("\nFilter test completed successfully!");
    }
}