using System;
using System.IO;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.Logging;

internal sealed class SkippedProductLogger : ISkippedProductLogger
{
    private const string OutputDirectory = "sealed-images";
    private const string LogFileName = "skipped.log";

    private readonly StreamWriter _writer;
    private readonly object _lock;
    private bool _disposed;

    public SkippedProductLogger()
    {
        _lock = new object();
        _disposed = false;

        if (Directory.Exists(OutputDirectory) is false)
        {
            _ = Directory.CreateDirectory(OutputDirectory);
        }

        string logPath = Path.Combine(OutputDirectory, LogFileName);
        _writer = new StreamWriter(logPath, append: false);
        _writer.WriteLine("SetCode\tName\tUuid\tReason");
        _writer.Flush();
    }

    public void LogSkipped(SealedProduct product, string reason)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            string escapedName = EscapeTabField(product.Name);
            string escapedReason = EscapeTabField(reason);
            _writer.WriteLine($"{product.SetCode}\t{escapedName}\t{product.Uuid}\t{escapedReason}");
            _writer.Flush();
        }
    }

    private static string EscapeTabField(string field)
    {
        if (string.IsNullOrEmpty(field))
        {
            return string.Empty;
        }

        return field
            .Replace("\t", " ", StringComparison.Ordinal)
            .Replace("\n", " ", StringComparison.Ordinal)
            .Replace("\r", " ", StringComparison.Ordinal);
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
