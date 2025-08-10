using Example.Core;
using Lib.Scryfall.Ingestion.Apis.Configurations;
using Lib.Scryfall.Ingestion.Configurations;
using Lib.Universal.Configurations;
using Microsoft.Extensions.Configuration;

namespace Example.Scryfall.ConfigDemo;

public sealed class ScryfallConfigDemoApplication : ExampleApplication
{
    protected override async Task Execute()
    {
        Log("=== Scryfall Configuration Demo ===");
        Log("");

        // Get the configuration from MonoStateConfig
        IConfig config = new MonoStateConfig();
        IScryfallConfiguration scryfallConfig = new ConfigScryfallConfiguration(config);

        try
        {
            await DisplayConfiguration(scryfallConfig).ConfigureAwait(false);
            await TestMissingConfiguration().ConfigureAwait(false);
            await TestInvalidConfiguration().ConfigureAwait(false);
        }
        catch (ScryfallConfigurationException ex)
        {
            Log($"❌ Error: {ex.Message}");
        }
    }

    private async Task DisplayConfiguration(IScryfallConfiguration scryfallConfig)
    {
        await Task.Run(() =>
        {
            // Display API Configuration
            Log("API Configuration:");
            Log("==================");
            IScryfallApiConfig apiConfig = scryfallConfig.ApiConfig();
            Log($"Base URL: {apiConfig.BaseUrl().AsSystemType()}");
            Log($"Timeout: {apiConfig.TimeoutSeconds().AsSystemType()} seconds");
            Log("");

            // Display Rate Limit Configuration
            Log("Rate Limit Configuration:");
            Log("========================");
            IScryfallRateLimitConfig rateLimitConfig = scryfallConfig.RateLimitConfig();
            Log($"Requests Per Second: {rateLimitConfig.RequestsPerSecond().AsSystemType()}");
            Log($"Burst Size: {rateLimitConfig.BurstSize().AsSystemType()}");
            Log($"Throttle Warning Threshold: {rateLimitConfig.ThrottleWarningThreshold().AsSystemType():P0}");
            Log("");

            // Display Cache Configuration
            Log("Cache Configuration:");
            Log("===================");
            IScryfallCacheConfig cacheConfig = scryfallConfig.CacheConfig();
            Log($"Card TTL: {cacheConfig.CardTtlHours().AsSystemType()} hours");
            Log($"Set TTL: {cacheConfig.SetTtlHours().AsSystemType()} hours");
            Log($"Max Cache Size: {cacheConfig.MaxCacheSize().AsSystemType()} MB");
            Log("");

            // Display Retry Configuration
            Log("Retry Configuration:");
            Log("===================");
            IScryfallRetryConfig retryConfig = scryfallConfig.RetryConfig();
            Log($"Max Retries: {retryConfig.MaxRetries().AsSystemType()}");
            Log($"Initial Delay: {retryConfig.InitialDelayMs().AsSystemType()} ms");
            Log($"Max Delay: {retryConfig.MaxDelayMs().AsSystemType()} ms");
            Log($"Backoff Multiplier: {retryConfig.BackoffMultiplier().AsSystemType()}x");
            Log("");

            Log("✅ Configuration loaded successfully!");
        }).ConfigureAwait(false);
    }

    private async Task TestMissingConfiguration()
    {
        await Task.Run(() =>
        {
            Log("");
            Log("=== Testing Missing Configuration ===");
            Log("Simulating missing configuration value to test error handling...");
            Log("");

            // Create a new configuration without BaseUrl
            IConfigurationRoot incompleteConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ScryfallConfig:Api:TimeoutSeconds"] = "30",
                    // BaseUrl is missing!
                    ["ScryfallConfig:RateLimit:RequestsPerSecond"] = "10"
                })
                .Build();

            // Create a test config wrapper
            TestConfig testConfig = new(incompleteConfig);
            IScryfallConfiguration incompleteScryfallConfig = new ConfigScryfallConfiguration(testConfig);

            try
            {
                string baseUrl = incompleteScryfallConfig.ApiConfig().BaseUrl().AsSystemType();
                Log($"❌ Should have thrown exception but got: {baseUrl}");
            }
            catch (ScryfallConfigurationException ex)
            {
                Log($"✅ Correctly caught missing configuration: {ex.Message}");
            }
        }).ConfigureAwait(false);
    }

    private async Task TestInvalidConfiguration()
    {
        await Task.Run(() =>
        {
            Log("");
            Log("=== Testing Invalid Configuration Value ===");
            Log("");

            IConfigurationRoot invalidConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ScryfallConfig:Api:TimeoutSeconds"] = "999", // Too high!
                    ["ScryfallConfig:Api:BaseUrl"] = "https://api.scryfall.com"
                })
                .Build();

            TestConfig testConfig = new(invalidConfig);
            IScryfallConfiguration invalidScryfallConfig = new ConfigScryfallConfiguration(testConfig);

            try
            {
                int timeout = invalidScryfallConfig.ApiConfig().TimeoutSeconds().AsSystemType();
                Log($"❌ Should have thrown exception but got: {timeout}");
            }
            catch (ScryfallConfigurationException ex)
            {
                Log($"✅ Correctly caught invalid value: {ex.Message}");
            }
        }).ConfigureAwait(false);
    }

    // Test wrapper for IConfig to use with test configurations
    private sealed class TestConfig : IConfig
    {
        private readonly IConfiguration _configuration;

        public TestConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string this[string key]
        {
            get => _configuration[key] ?? string.Empty;
            set => _configuration[key] = value;
        }
    }
}
