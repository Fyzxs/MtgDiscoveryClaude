using System;
using System.IO;
using System.Threading;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.Logging;

internal sealed class SkippedProductLogger : ISkippedProductLogger
{
    private const string OutputDirectory = "sealed-images";
    private const string KnownLogFileName = "skipped-known.log";
    private const string NoImageLogFileName = "skipped-image.log";
    private const string UnknownLogFileName = "skipped-unknown.log";
    private const string LogHeader = "SetCode\tName\tUuid\tCategory\tSubtype\tReason";

    private readonly StreamWriter _knownWriter;
    private readonly StreamWriter _noImageWriter;
    private readonly StreamWriter _unknownWriter;
    private readonly Lock _lock;
    private bool _disposed;

    public SkippedProductLogger()
    {
        _lock = new Lock();
        _disposed = false;

        if (Directory.Exists(OutputDirectory) is false)
        {
            _ = Directory.CreateDirectory(OutputDirectory);
        }

        _knownWriter = CreateWriter(KnownLogFileName);
        _noImageWriter = CreateWriter(NoImageLogFileName);
        _unknownWriter = CreateWriter(UnknownLogFileName);
    }

    private static StreamWriter CreateWriter(string fileName)
    {
        string logPath = Path.Combine(OutputDirectory, fileName);
        StreamWriter writer = new(logPath, append: false);
        writer.WriteLine(LogHeader);
        writer.Flush();
        return writer;
    }

    public void LogSkippedKnown(SealedProduct product, string reason) => WriteLog(_knownWriter, product, reason);

    public void LogSkippedNoImage(SealedProduct product) => WriteLog(_noImageWriter, product, "No image found from any provider");

    public void LogSkippedUnknown(SealedProduct product, string reason) => WriteLog(_unknownWriter, product, reason);

    private void WriteLog(StreamWriter writer, SealedProduct product, string reason)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            string escapedName = EscapeTabField(product.Name);
            string escapedCategory = EscapeTabField(product.Category);
            string escapedSubtype = EscapeTabField(product.Subtype);
            string escapedReason = EscapeTabField(reason);
            writer.WriteLine($"{product.SetCode}\t{escapedName}\t{product.Uuid}\t{escapedCategory}\t{escapedSubtype}\t{escapedReason}");
            writer.Flush();
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
            _knownWriter.Dispose();
            _noImageWriter.Dispose();
            _unknownWriter.Dispose();
        }
    }
}
