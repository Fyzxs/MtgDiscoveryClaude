using System;
using System.IO;
using System.Threading;
using Cli.MtgDiscovery.PriceUpdate.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.PriceUpdate.ErrorTracking;

internal sealed class CsvPriceUpdateErrorLogger : IPriceUpdateErrorLogger
{
    private readonly ILogger<CsvPriceUpdateErrorLogger> _logger;
    private readonly StreamWriter _writer;
    private readonly Lock _lock;
    private bool _disposed;

    public CsvPriceUpdateErrorLogger(
        ILogger<CsvPriceUpdateErrorLogger> logger,
        PriceUpdateConfiguration config)
    {
        _logger = logger;
        _lock = new Lock();
        _disposed = false;

        string directory = Path.GetDirectoryName(config.ErrorOutputPath) ?? ".";
        if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
        {
            _ = Directory.CreateDirectory(directory);
        }

        _writer = new StreamWriter(config.ErrorOutputPath, append: false);
        _writer.WriteLine("scryfall_id,container,error_message");
        _writer.Flush();

        _logger.LogInformation("Error log initialized: {Path}", config.ErrorOutputPath);
    }

    public void LogError(PriceUpdateError error)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            string escapedMessage = EscapeCsvField(error.ErrorMessage);
            _writer.WriteLine($"{error.ScryfallId},{error.Container},{escapedMessage}");
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
