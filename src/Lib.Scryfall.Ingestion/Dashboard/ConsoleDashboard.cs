using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Dashboard;

internal sealed class ConsoleDashboard : IIngestionDashboard
{
    private readonly Lock _lock = new();
    private readonly Queue<string> _recentLogs = new(3);
    private readonly Dictionary<string, int> _completedCounts = [];
    private readonly Stopwatch _stopwatch = new();
    private readonly char[] _spinnerChars = ['⠋', '⠙', '⠹', '⠸', '⠼', '⠴', '⠦', '⠧', '⠇', '⠏'];
    private readonly int _refreshFrequency;
    private readonly bool _enableMemoryThrottling;
    private int _updateCounter;
    private int _spinnerIndex;

    // Unified progress tracking
    private string _progressType = string.Empty;
    private int _progressCurrent;
    private int _progressTotal;
    private string _progressAction = string.Empty;
    private string _progressItem = string.Empty;

    // Legacy tracking (to be removed)
    private int _setCurrent;
    private int _setTotal;
    private string _setName = string.Empty;
    private int _cardCurrent;
    private int _cardTotal;
    private string _cardName = string.Empty;
    private int _rulingCurrent;
    private int _rulingTotal;
    private string _rulingName = string.Empty;
    private int _trigramCurrent;
    private int _trigramTotal;
    private string _trigramName = string.Empty;
    private long _memoryUsage;
    private int _lastHeight;
    private int _lastWidth;
    private bool _isComplete;

    public ConsoleDashboard(int refreshFrequency = 100, bool enableMemoryThrottling = true)
    {
        _refreshFrequency = refreshFrequency;
        _enableMemoryThrottling = enableMemoryThrottling;
    }

    public void SetStartTime() => _stopwatch.Start();

    public void UpdateProgress(string type, int current, int total, string action, string item)
    {
        lock (_lock)
        {
            _progressType = type ?? string.Empty;
            _progressCurrent = current;
            _progressTotal = total;
            _progressAction = action ?? string.Empty;
            _progressItem = item ?? string.Empty;
            CheckAndRefresh();
        }
    }

    public void UpdateSetProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _setCurrent = current;
            _setTotal = total;
            _setName = name ?? string.Empty;
            CheckAndRefresh();
        }
    }

    public void UpdateCardProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _cardCurrent = current;
            _cardTotal = total;
            _cardName = name ?? string.Empty;
            CheckAndRefresh();
        }
    }

    public void UpdateRulingProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _rulingCurrent = current;
            _rulingTotal = total;
            _rulingName = name ?? string.Empty;
            CheckAndRefresh();
        }
    }

    public void UpdateTrigramProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _trigramCurrent = current;
            _trigramTotal = total;
            _trigramName = name ?? string.Empty;
            CheckAndRefresh();
        }
    }

    public void AddCompletedSet(string name)
    {
        // No longer tracking completed sets
    }

    public void UpdateCompletedCount(string type, int count)
    {
        lock (_lock)
        {
            _completedCounts[type] = count;
            CheckAndRefresh();
        }
    }

    public void UpdateMemoryUsage()
    {
        if (_enableMemoryThrottling)
        {
            _memoryUsage = GC.GetTotalMemory(false) / (1024 * 1024); // MB
        }
    }

    private void CheckAndRefresh()
    {
        _updateCounter++;

        if (_enableMemoryThrottling)
        {
            UpdateMemoryUsage();
        }

        if (_updateCounter >= _refreshFrequency)
        {
            _updateCounter = 0;
            RefreshInternal();
        }
    }

    public void Complete(string message)
    {
        lock (_lock)
        {
            _isComplete = true;
            // Do one final refresh to show the completed state
            ClearDashboard();
            DrawDashboard();

            // Print completion message below the dashboard
            Console.WriteLine();
            Console.WriteLine($"  {message}");
            Console.WriteLine();
        }
    }

    public void Refresh()
    {
        lock (_lock)
        {
            RefreshInternal();
        }
    }

    private void RefreshInternal()
    {
        if (_isComplete) return;
        ClearDashboard();
        DrawDashboard();
    }

    private void ClearDashboard()
    {
        if (_lastHeight > 0)
        {
            // Use the wider of last width or current width to ensure complete clearing
            int currentWidth = Math.Max(80, Console.WindowWidth - 1);
            int clearWidth = Math.Max(_lastWidth, currentWidth);

            // Clear a few extra lines above to handle wrapped content
            int extraLines = 3;
            int totalLinesToClear = _lastHeight + extraLines;

            Console.SetCursorPosition(0, Math.Max(0, Console.CursorTop - totalLinesToClear));
            for (int i = 0; i < totalLinesToClear; i++)
            {
                Console.WriteLine(new string(' ', clearWidth));
            }

            Console.SetCursorPosition(0, Math.Max(0, Console.CursorTop - totalLinesToClear + extraLines));
        }
    }

    private void DrawDashboard()
    {
        List<string> lines = [];
        int width = Math.Max(80, Console.WindowWidth - 1);
        _lastWidth = width; // Track width for proper clearing on next refresh

        lines.Add(new string('═', width));
        lines.Add("  Scryfall Bulk Ingestion Progress");
        lines.Add(new string('═', width));

        // Unified progress display
        if (!string.IsNullOrEmpty(_progressType))
        {
            if (_progressTotal > 0)
            {
                string progressBar = CreateProgressBar(_progressCurrent, _progressTotal, width);
                int percent = _progressCurrent * 100 / _progressTotal;
                string typePadded = _progressType.PadRight(10);
                lines.Add($"  {typePadded} {progressBar} {_progressCurrent:N0}/{_progressTotal:N0} ({percent}%)");
            }
            else if (_progressCurrent > 0)
            {
                // When streaming, we don't know the total upfront
                string spinner = GetSpinner();
                string typePadded = _progressType.PadRight(10);
                lines.Add($"  {typePadded} Processing... {spinner} {_progressCurrent:N0} items");
            }
            else
            {
                string typePadded = _progressType.PadRight(10);
                lines.Add($"  {typePadded} Waiting to start...");
            }

            if (!string.IsNullOrEmpty(_progressAction) || !string.IsNullOrEmpty(_progressItem))
            {
                string currentItem = string.IsNullOrEmpty(_progressItem)
                    ? _progressAction
                    : $"{_progressAction} - {_progressItem}";
                lines.Add($"  Current Item:   {TruncateString(currentItem, width - 18)}");
            }
        }
        else
        {
            // Fall back to legacy display if no unified progress is set
            if (_setTotal > 0)
            {
                string setProgress = CreateProgressBar(_setCurrent, _setTotal, width);
                int setPercent = _setCurrent * 100 / _setTotal;
                lines.Add($"  Sets:           {setProgress} {_setCurrent}/{_setTotal} ({setPercent}%)");
                lines.Add($"  Current Item:   {TruncateString(_setName, width - 18)}");
            }
            else if (_cardTotal > 0 || _cardCurrent > 0)
            {
                if (_cardTotal > 0)
                {
                    string cardProgress = CreateProgressBar(_cardCurrent, _cardTotal, width);
                    int cardPercent = _cardCurrent * 100 / _cardTotal;
                    lines.Add($"  Cards:          {cardProgress} {_cardCurrent:N0}/{_cardTotal:N0} ({cardPercent}%)");
                }
                else
                {
                    string spinner = GetSpinner();
                    lines.Add($"  Cards:          Processing... {spinner} {_cardCurrent:N0} cards");
                }

                lines.Add($"  Current Item:   {TruncateString(_cardName, width - 18)}");
            }
            else if (_trigramTotal > 0 || _trigramCurrent > 0)
            {
                if (_trigramTotal > 0)
                {
                    string trigramProgress = CreateProgressBar(_trigramCurrent, _trigramTotal, width);
                    int trigramPercent = _trigramCurrent * 100 / _trigramTotal;
                    lines.Add($"  Trigrams:       {trigramProgress} {_trigramCurrent:N0}/{_trigramTotal:N0} ({trigramPercent}%)");
                }
                else
                {
                    string spinner = GetSpinner();
                    lines.Add($"  Trigrams:       Processing... {spinner} {_trigramCurrent:N0} trigrams");
                }

                lines.Add($"  Current Item:   {TruncateString(_trigramName, width - 18)}");
            }
            else if (_rulingTotal > 0 || _rulingCurrent > 0)
            {
                if (_rulingTotal > 0)
                {
                    string rulingProgress = CreateProgressBar(_rulingCurrent, _rulingTotal, width);
                    int rulingPercent = _rulingCurrent * 100 / _rulingTotal;
                    lines.Add($"  Rulings:        {rulingProgress} {_rulingCurrent:N0}/{_rulingTotal:N0} ({rulingPercent}%)");
                }
                else
                {
                    string spinner = GetSpinner();
                    lines.Add($"  Rulings:        Processing... {spinner} {_rulingCurrent:N0} rulings");
                }

                lines.Add($"  Current Item:   {TruncateString(_rulingName, width - 18)}");
            }
            else
            {
                lines.Add("  Status:         Waiting to start...");
            }
        }

        // Display completed counts tracker
        if (_completedCounts.Count > 0)
        {
            lines.Add("");
            lines.Add("  Completed:");
            string completedLine = "  ";
            foreach (KeyValuePair<string, int> kvp in _completedCounts.OrderBy(x => GetTypeOrder(x.Key)))
            {
                if (completedLine.Length > 2) completedLine += " | ";
                completedLine += $"{kvp.Key}: {kvp.Value:N0}";
            }

            lines.Add(completedLine);
        }

        if (_recentLogs.Count > 0)
        {
            lines.Add("");
            lines.Add("  Recent Activity:");
            foreach (string log in _recentLogs)
            {
                lines.Add($"  {TruncateString(log, width - 4)}");
            }
        }

        lines.Add(new string('═', width));
        string system = $"  Memory: {_memoryUsage} MB | Elapsed: {FormatElapsed()}";
        lines.Add(system);
        lines.Add(new string('═', width));

        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }

        _lastHeight = lines.Count;
    }

    private string GetSpinner()
    {
        _spinnerIndex = (_spinnerIndex + 1) % _spinnerChars.Length;
        return _spinnerChars[_spinnerIndex].ToString();
    }

    private static string CreateProgressBar(int current, int total, int consoleWidth)
    {
        // Progress bar should be about 1/3 of console width, with min 20 and max 80
        int barWidth = Math.Min(80, Math.Max(20, consoleWidth / 3));
        int filled = current * barWidth / total;
        int empty = barWidth - filled;

        return $"[{new string('█', filled)}{new string('░', empty)}]";
    }

    private static string TruncateString(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        if (text.Length <= maxLength) return text;
        return string.Concat(text.AsSpan(0, maxLength - 3), "...");
    }

    private string FormatElapsed()
    {
        TimeSpan elapsed = _stopwatch.Elapsed;
        return $"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";
    }

    public CancellationToken GetCancellationToken() => CancellationToken.None;

    // ILogger implementation
    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        string message = formatter(state, exception);
        if (string.IsNullOrWhiteSpace(message)) return;

        lock (_lock)
        {
            if (_recentLogs.Count >= 3) { _recentLogs.Dequeue(); }

            _recentLogs.Enqueue($"[{logLevel}] {message}");
        }

        Refresh();
    }

    private static int GetTypeOrder(string type)
    {
        return type switch
        {
            "Sets" => 1,
            "Cards" => 2,
            "Rulings" => 3,
            "Artists" => 4,
            "Card Trigrams" => 5,
            "Artist Trigrams" => 6,
            _ => 99
        };
    }

    private sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        public void Dispose() { }
    }
}
