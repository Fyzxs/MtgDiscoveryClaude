using System;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.Migration;

internal sealed class MigrationProgressTracker : IMigrationProgressTracker
{
    private readonly ILogger _logger;
    private int _totalRecords;
    private int _currentProgress;
    private readonly object _lock;

    public MigrationProgressTracker(ILogger logger)
    {
        _logger = logger;
        _totalRecords = 0;
        _currentProgress = 0;
        _lock = new object();
    }

    public void Initialize(int totalRecords)
    {
        _totalRecords = totalRecords;
        _currentProgress = 0;
        Console.WriteLine($"Starting migration of {_totalRecords} records...");
        DisplayProgress();
    }

    public void IncrementProgress()
    {
        lock (_lock)
        {
            _currentProgress++;
            DisplayProgress();
        }
    }

    public void Complete()
    {
        Console.WriteLine();
        Console.WriteLine($"Migration complete: {_currentProgress}/{_totalRecords} records processed.");
    }

    private void DisplayProgress()
    {
        if (_totalRecords == 0)
        {
            return;
        }

        int percentage = (_currentProgress * 100) / _totalRecords;
        int barLength = 50;
        int filledLength = (percentage * barLength) / 100;

        string bar = new string('=', filledLength);
        string empty = new string('-', barLength - filledLength);

        Console.Write($"\r[{bar}{empty}] {percentage}% ({_currentProgress}/{_totalRecords})");
    }
}
