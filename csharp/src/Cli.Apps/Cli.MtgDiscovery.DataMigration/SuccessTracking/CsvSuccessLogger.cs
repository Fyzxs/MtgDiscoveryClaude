using System;
using System.IO;
using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.Configuration;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.SuccessTracking;

internal sealed class CsvSuccessLogger : ISuccessLogger, IDisposable
{
    private readonly ILogger _logger;
    private readonly StreamWriter _writer;
    private bool _headerWritten;

    public CsvSuccessLogger(ILogger logger, MigrationConfiguration configuration)
    {
        _logger = logger;
        string filePath = configuration.SuccessOutputPath;
        _writer = new StreamWriter(filePath, append: false);
        _headerWritten = false;
    }

    public async Task LogSuccessAsync(MigrationSuccess success)
    {
        if (_headerWritten is false)
        {
            await WriteHeaderAsync().ConfigureAwait(false);
            _headerWritten = true;
        }

        string csvLine = FormatAsCsvLine(success);
        await _writer.WriteLineAsync(csvLine).ConfigureAwait(false);
    }

    public async Task FlushAsync()
    {
        await _writer.FlushAsync().ConfigureAwait(false);
        _writer.Close();
    }

    private async Task WriteHeaderAsync()
    {
        string header = "OldCardId,ScryfallId,SetId,Finish,Special,SetGroupId,Count";
        await _writer.WriteLineAsync(header).ConfigureAwait(false);
    }

    private string FormatAsCsvLine(MigrationSuccess success)
    {
        string oldCardId = EscapeCsvValue(success.OldCardId);
        string scryfallId = EscapeCsvValue(success.ScryfallId);
        string setId = EscapeCsvValue(success.SetId);
        string finish = EscapeCsvValue(success.Finish);
        string special = EscapeCsvValue(success.Special);
        string setGroupId = EscapeCsvValue(success.SetGroupId);
        string count = success.Count.ToString();

        return $"{oldCardId},{scryfallId},{setId},{finish},{special},{setGroupId},{count}";
    }

    private string EscapeCsvValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Contains(',', StringComparison.OrdinalIgnoreCase) || value.Contains('"', StringComparison.OrdinalIgnoreCase) || value.Contains('\n', StringComparison.OrdinalIgnoreCase))
        {
            string escaped = value.Replace("\"", "\"\"", StringComparison.OrdinalIgnoreCase);
            return $"\"{escaped}\"";
        }

        return value;
    }

    public void Dispose() => _writer?.Dispose();
}
