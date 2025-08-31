using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lib.Scryfall.Ingestion.Apis.Dashboard;

namespace Lib.Scryfall.Ingestion.Dashboard;

internal sealed class ConsoleDashboard : IIngestionDashboard
{
    private readonly object _lock = new();
    private readonly Queue<string> _recentSets = new(5);
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
            ClearDashboard();
            Console.WriteLine(message);
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
            Console.SetCursorPosition(0, Math.Max(0, Console.CursorTop - _lastHeight));
            for (int i = 0; i < _lastHeight; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth - 1));
            }
            Console.SetCursorPosition(0, Math.Max(0, Console.CursorTop - _lastHeight));
        }
    }

    private void DrawDashboard()
    {
        List<string> lines = new List<string>();

        lines.Add("═══════════════════════════════════════════════════════════");
        lines.Add("  Scryfall Bulk Ingestion Progress");
        lines.Add("═══════════════════════════════════════════════════════════");

        if (_setTotal > 0)
        {
            string setProgress = CreateProgressBar(_setCurrent, _setTotal);
            int setPercent = (_setCurrent * 100) / _setTotal;
            lines.Add($"  Sets:     {setProgress} {_setCurrent}/{_setTotal} ({setPercent}%)");
            lines.Add($"  Current:  {TruncateString(_setName, 50)}");
        }
        else
        {
            lines.Add("  Sets:     Waiting to start...");
        }

        lines.Add("");

        if (_cardTotal > 0)
        {
            string cardProgress = CreateProgressBar(_cardCurrent, _cardTotal);
            int cardPercent = (_cardCurrent * 100) / _cardTotal;
            lines.Add($"  Cards:    {cardProgress} {_cardCurrent:N0}/{_cardTotal:N0} ({cardPercent}%)");
            lines.Add($"  Current:  {TruncateString(_cardName, 50)}");
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
                lines.Add($"  ✓ {TruncateString(set, 55)}");
            }
        }

        lines.Add("═══════════════════════════════════════════════════════════");

        string stats = $"  Artists: {_artistCount:N0} | Rulings: {_rulingCount:N0}";
        string system = $"  Memory: {_memoryUsage} MB | Elapsed: {FormatElapsed()}";
        lines.Add(stats);
        lines.Add(system);
        lines.Add("═══════════════════════════════════════════════════════════");

        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }

        _lastHeight = lines.Count;
    }

    private static string CreateProgressBar(int current, int total)
    {
        const int width = 20;
        int filled = (current * width) / total;
        int empty = width - filled;

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
}