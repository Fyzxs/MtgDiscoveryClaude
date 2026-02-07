using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cli.MtgDiscovery.UserDataReconciler.Dashboard;
using Cli.MtgDiscovery.UserDataReconciler.Reconciliation;
using Example.Core;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitors;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.UserDataReconciler;

internal sealed class ReconcilerApplication : ExampleApplication
{
    private string[] _args = [];

    public new async Task StartUp(string[] args)
    {
        _args = args;
        await base.StartUp(args).ConfigureAwait(false);
    }

    protected override async Task Execute()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            _ = builder.AddConsole();
            _ = builder.SetMinimumLevel(LogLevel.Warning);
        });

        ILogger logger = loggerFactory.CreateLogger<ReconcilerApplication>();

        ReconcilerConfiguration config = Configuration
            .GetSection("ReconcilerConfiguration")
            .Get<ReconcilerConfiguration>();

        if (config is null)
        {
            logger.LogError("ReconcilerConfiguration is missing");
            throw new InvalidOperationException("ReconcilerConfiguration must be configured");
        }

        // Allow --live flag to override DryRun
        if (_args.Contains("--live", StringComparer.OrdinalIgnoreCase))
        {
            config = new ReconcilerConfiguration
            {
                DryRun = false,
                SpecificUserId = config.SpecificUserId
            };
        }

        using RazorConsoleReconcilerDashboard dashboard = new();

        Task reconciliationTask = Task.Run(async () =>
        {
            try
            {
                await RunReconciliationAsync(config, dashboard, logger).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Swallow exception to pass to UI
            catch (Exception ex)
#pragma warning restore CA1031
            {
                logger.LogError(ex, "Fatal error during reconciliation");
                dashboard.MarkComplete($"Failed: {ex.Message}");
            }
        });

        await dashboard.RunUiAsync().ConfigureAwait(false);
        await reconciliationTask.ConfigureAwait(false);
    }

    private static async Task RunReconciliationAsync(
        ReconcilerConfiguration config,
        IReconcilerDashboard dashboard,
        ILogger logger)
    {
        dashboard.SetMode(config.DryRun);
        dashboard.StartTimer();

        List<string> userIds;
        if (string.IsNullOrWhiteSpace(config.SpecificUserId) is false)
        {
            dashboard.SetStatus($"Processing specific user: {config.SpecificUserId}");
            dashboard.AddLog($"Single user mode: {config.SpecificUserId}");
            userIds = [config.SpecificUserId];
        }
        else
        {
            dashboard.SetStatus("Discovering users...");
            userIds = await GetAllUserIdsAsync(logger).ConfigureAwait(false);
            dashboard.AddLog($"Found {userIds.Count} users to process");
        }

        dashboard.SetTotalUsers(userIds.Count);
        dashboard.SetStatus("Processing users...");

        UserSetCardsReconciler reconciler = new(logger, config.DryRun, dashboard);

        int processedCount = 0;
        foreach (string userId in userIds)
        {
            if (dashboard.GetCancellationToken().IsCancellationRequested)
            {
                dashboard.AddLog("Cancellation requested - stopping");
                break;
            }

            processedCount++;
            dashboard.StartUser(userId, processedCount);

            ReconciliationSummary userSummary = await reconciler.ReconcileUserAsync(userId).ConfigureAwait(false);

            if (userSummary.SetsProcessed > 0)
            {
                if (userSummary.SetsWithDiscrepancies > 0)
                {
                    dashboard.AddLog($"User {processedCount}: {userSummary.SetsWithDiscrepancies} sets with discrepancies");
                }
            }
        }

        string completionMessage = dashboard.GetCancellationToken().IsCancellationRequested
            ? "Reconciliation cancelled by user"
            : "Reconciliation complete!";

        dashboard.SetStatus(completionMessage);
        dashboard.MarkComplete(completionMessage);
    }

    private static async Task<List<string>> GetAllUserIdsAsync(ILogger logger)
    {
        UserSetCardsInquisitor inquisitor = new(logger);

        QueryDefinition query = new("SELECT DISTINCT VALUE c.partition FROM c");

        OpResponse<IEnumerable<string>> response = await inquisitor
            .QueryAsync<string>(query, PartitionKey.None)
            .ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            throw new InvalidOperationException($"Failed to query user IDs: {response.StatusCode}");
        }

        return [.. response.Value];
    }
}
