using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Dashboard;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Dashboard;

internal sealed class ConsoleDashboard : IIngestionDashboard
{
    private readonly object _lock = new();
    private readonly Queue<string> _recentSets = new(5);
    private readonly Queue<string> _recentLogs = new(3);
    private readonly Stopwatch _stopwatch = new();
    private int _setCurrent;
    private int _setTotal;
    private string _setName = string.Empty;
    private int _cardCurrent;
    private int _cardTotal;
    private string _cardName = string.Empty;
    private int _artistCount;
    private int _rulingCount;
    private long _memoryUsage;
    private int _lastHeight;
    private int _lastWidth;
    private bool _isComplete;

    public void SetStartTime()
    {
        _stopwatch.Start();
    }

    public void UpdateSetProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _setCurrent = current;
            _setTotal = total;
            _setName = name ?? string.Empty;
        }
    }

    public void UpdateCardProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _cardCurrent = current;
            _cardTotal = total;
            _cardName = name ?? string.Empty;
        }
    }

    public void UpdateArtistCount(int count)
    {
        lock (_lock)
        {
            _artistCount = count;
        }
    }

    public void UpdateRulingCount(int count)
    {
        lock (_lock)
        {
            _rulingCount = count;
        }
    }

    public void AddCompletedSet(string name)
    {
        lock (_lock)
        {
            if (_recentSets.Count >= 5) { _recentSets.Dequeue(); }
            _recentSets.Enqueue(name);
        }
    }

    public void UpdateMemoryUsage()
    {
        _memoryUsage = GC.GetTotalMemory(false) / (1024 * 1024); // MB
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
        if (_isComplete) return;

        lock (_lock)
        {
            ClearDashboard();
            DrawDashboard();
        }
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
        List<string> lines = new List<string>();
        int width = Math.Max(80, Console.WindowWidth - 1);
        _lastWidth = width; // Track width for proper clearing on next refresh

        lines.Add(new string('═', width));
        lines.Add("  Scryfall Bulk Ingestion Progress");
        lines.Add(new string('═', width));

        if (_setTotal > 0)
        {
            string setProgress = CreateProgressBar(_setCurrent, _setTotal, width);
            int setPercent = (_setCurrent * 100) / _setTotal;
            lines.Add($"  Sets:     {setProgress} {_setCurrent}/{_setTotal} ({setPercent}%)");
            lines.Add($"  Current:  {TruncateString(_setName, width - 12)}");
        }
        else
        {
            lines.Add("  Sets:     Waiting to start...");
        }

        lines.Add("");

        if (_cardTotal > 0)
        {
            string cardProgress = CreateProgressBar(_cardCurrent, _cardTotal, width);
            int cardPercent = (_cardCurrent * 100) / _cardTotal;
            lines.Add($"  Cards:    {cardProgress} {_cardCurrent:N0}/{_cardTotal:N0} ({cardPercent}%)");
            lines.Add($"  Current:  {TruncateString(_cardName, width - 12)}");
        }
        else
        {
            lines.Add("  Cards:    Not started");
        }

        if (_recentSets.Count > 0)
        {
            lines.Add("");
            lines.Add("  Recently Completed Sets:");
            foreach (string set in _recentSets)
            {
                lines.Add($"  ✓ {TruncateString(set, width - 6)}");
            }
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

        string stats = $"  Artists: {_artistCount:N0} | Rulings: {_rulingCount:N0}";
        string system = $"  Memory: {_memoryUsage} MB | Elapsed: {FormatElapsed()}";
        lines.Add(stats);
        lines.Add(system);
        lines.Add(new string('═', width));

        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }

        _lastHeight = lines.Count;
    }

    private static string CreateProgressBar(int current, int total, int consoleWidth)
    {
        // Progress bar should be about 1/3 of console width, with min 20 and max 80
        int barWidth = Math.Min(80, Math.Max(20, consoleWidth / 3));
        int filled = (current * barWidth) / total;
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

    private sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        public void Dispose() { }
    }
}