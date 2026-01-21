using System;
using System.Diagnostics;
using Azure.Core;
using Azure.Identity;
using Lib.Universal.Configurations;
using Lib.Universal.Inversion;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL;

internal static class AppMtgDiscoveryGraphQlProgram
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                _ = config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                _ = config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _ = config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                _ = config.AddEnvironmentVariables();

                ConfigureAppConfiguration(hostingContext, config);
            })
            .ConfigureLogging(loggingBuilder =>
            {
                _ = loggingBuilder.ClearProviders();
                _ = loggingBuilder.AddConsole();
                _ = loggingBuilder.AddApplicationInsights();
            }).ConfigureServices((builder, _) =>
            {
                EntraAuth();

                MonoStateConfig.SetConfiguration(builder.Configuration);
            })
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }

    [Conditional("RELEASE")]
    private static void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
    {
        if (hostingContext.HostingEnvironment.IsEnvironment("Local")) return;

        IConfigurationRoot tempConfig = config.Build();
        string appConfigEndpoint = tempConfig["AppConfiguration:Endpoint"];

        if (string.IsNullOrEmpty(appConfigEndpoint)) return;

        _ = config.AddAzureAppConfiguration(options =>
        {
            _ = options.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential())
                .Select("Auth0:*");
        });
    }

    [Conditional("RELEASE")]
    private static void EntraAuth()
    {
        DefaultAzureCredential defaultAzureCredential = new();
        ServiceLocator.ServiceRegister<TokenCredential>(() => defaultAzureCredential);
    }
}
