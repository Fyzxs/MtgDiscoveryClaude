using System;
using System.IO;
using System.Threading;
using Cli.MtgDiscovery.PriceUpdate.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.PriceChangeLogging;

internal sealed class CsvPriceChangeLogger : IPriceChangeLogger
{
    private readonly ILogger<CsvPriceChangeLogger> _logger;
    private readonly StreamWriter _writer;
    private readonly Lock _lock;
    private bool _disposed;

    public CsvPriceChangeLogger(
        ILogger<CsvPriceChangeLogger> logger,
        PriceUpdateConfiguration config)
    {
        _logger = logger;
        _lock = new Lock();
        _disposed = false;

        string directory = Path.GetDirectoryName(config.PriceChangeLogPath) ?? ".";
        if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
        {
            _ = Directory.CreateDirectory(directory);
        }

        _writer = new StreamWriter(config.PriceChangeLogPath, append: false);
        _writer.WriteLine("scryfall_id,container,card_name,set_code,old_usd,old_usd_foil,old_usd_etched,new_usd,new_usd_foil,new_usd_etched");
        _writer.Flush();

        _logger.LogInformation("Price change log initialized: {Path}", config.PriceChangeLogPath);
    }

    public void LogChange(PriceChangeRecord record)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            string escapedName = EscapeCsvField(record.CardName);
            _writer.WriteLine(
                $"{record.ScryfallId},{record.Container},{escapedName},{record.SetCode}," +
                $"{record.OldUsd},{record.OldUsdFoil},{record.OldUsdEtched}," +
                $"{record.NewUsd},{record.NewUsdFoil},{record.NewUsdEtched}");
            _writer.Flush();
        }
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',', StringComparison.Ordinal) ||
            field.Contains('"', StringComparison.Ordinal) ||
            field.Contains('\n', StringComparison.Ordinal))
        {
            return $"\"{field.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
        }

        return field;
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _writer.Dispose();
        }
    }
}
