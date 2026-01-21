using System.IO;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.ErrorTracking;

internal sealed class CsvErrorLogger : IErrorLogger
{
    private readonly ILogger _logger;
    private readonly string _filePath;
    private readonly StreamWriter _writer;
    private bool _headerWritten;

    public CsvErrorLogger(ILogger logger, MigrationConfiguration configuration)
    {
        _logger = logger;
        _filePath = configuration.ErrorOutputPath;
        _writer = new StreamWriter(_filePath, append: false);
        _headerWritten = false;
    }

    public async Task LogErrorAsync(MigrationError error)
    {
        if (_headerWritten is false)
        {
            await WriteHeaderAsync().ConfigureAwait(false);
            _headerWritten = true;
        }

        string csvLine = FormatAsCsvLine(error);
        await _writer.WriteLineAsync(csvLine).ConfigureAwait(false);
    }

    public async Task FlushAsync()
    {
        await _writer.FlushAsync().ConfigureAwait(false);
        _writer.Close();
    }

    private async Task WriteHeaderAsync()
    {
        string header = "OldCardId,ScryfallId,SetId,ErrorReason";
        await _writer.WriteLineAsync(header).ConfigureAwait(false);
    }

    private string FormatAsCsvLine(MigrationError error)
    {
        string oldCardId = EscapeCsvValue(error.OldCardId);
        string scryfallId = EscapeCsvValue(error.ScryfallId);
        string setId = EscapeCsvValue(error.SetId);
        string errorReason = EscapeCsvValue(error.ErrorReason);

        return $"{oldCardId},{scryfallId},{setId},{errorReason}";
    }

    private string EscapeCsvValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
        {
            string escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        return value;
    }
}
