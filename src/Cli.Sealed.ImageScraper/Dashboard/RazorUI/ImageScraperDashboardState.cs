using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Cli.Sealed.ImageScraper.Dashboard.RazorUI;

#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0044 // Make field readonly
internal sealed class ImageScraperDashboardState : IDisposable
{
    private readonly Lock _lock = new();
    private readonly Queue<string> _recentLogs = new(5);
    private readonly Stopwatch _stopwatch = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private string _currentSet = string.Empty;
    private int _setIndex;
    private int _totalSets;

    private int _current;
    private int _total;
    private string _currentProduct = string.Empty;

    private int _downloadedCount;
    private int _skippedCount;
    private int _errorCount;
    private int _noImageCount;

    private long _memoryUsage;
    private bool _isComplete;
    private string _completionMessage = string.Empty;
    private string _status = "Initializing...";

    public string CurrentSet
    {
        get { lock (_lock) { return _currentSet; } }
    }

    public int SetIndex
    {
        get { lock (_lock) { return _setIndex; } }
    }

    public int TotalSets
    {
        get { lock (_lock) { return _totalSets; } }
    }

    public int Current
    {
        get { lock (_lock) { return _current; } }
    }

    public int Total
    {
        get { lock (_lock) { return _total; } }
    }

    public string CurrentProduct
    {
        get { lock (_lock) { return _currentProduct; } }
    }

    public int DownloadedCount
    {
        get { lock (_lock) { return _downloadedCount; } }
    }

    public int SkippedCount
    {
        get { lock (_lock) { return _skippedCount; } }
    }

    public int ErrorCount
    {
        get { lock (_lock) { return _errorCount; } }
    }

    public int NoImageCount
    {
        get { lock (_lock) { return _noImageCount; } }
    }

    public long MemoryUsage
    {
        get { lock (_lock) { return _memoryUsage; } }
    }

    public TimeSpan Elapsed
    {
        get { lock (_lock) { return _stopwatch.Elapsed; } }
    }

    public bool IsComplete
    {
        get { lock (_lock) { return _isComplete; } }
    }

    public string CompletionMessage
    {
        get { lock (_lock) { return _completionMessage; } }
    }

    public string Status
    {
        get { lock (_lock) { return _status; } }
    }

    public IReadOnlyList<string> RecentLogs
    {
        get { lock (_lock) { return [.. _recentLogs]; } }
    }

    public void StartTimer()
    {
        lock (_lock)
        {
            _stopwatch.Start();
        }
    }

    public void SetStatus(string status)
    {
        lock (_lock)
        {
            _status = status ?? string.Empty;
        }
    }

    public void SetCurrentSet(string setCode, int setIndex, int totalSets)
    {
        lock (_lock)
        {
            _currentSet = setCode ?? string.Empty;
            _setIndex = setIndex;
            _totalSets = totalSets;
            _current = 0;
        }
    }

    public void SetTotalProducts(int total)
    {
        lock (_lock)
        {
            _total = total;
        }
    }

    public void UpdateProgress(int current, string productName)
    {
        lock (_lock)
        {
            _current = current;
            _currentProduct = productName ?? string.Empty;
        }
    }

    public void IncrementDownloaded()
    {
        lock (_lock)
        {
            _downloadedCount++;
        }
    }

    public void IncrementSkipped()
    {
        lock (_lock)
        {
            _skippedCount++;
        }
    }

    public void IncrementError()
    {
        lock (_lock)
        {
            _errorCount++;
        }
    }

    public void IncrementNoImage()
    {
        lock (_lock)
        {
            _noImageCount++;
        }
    }

    public void UpdateMemoryUsage()
    {
        lock (_lock)
        {
            _memoryUsage = GC.GetTotalMemory(false) / (1024 * 1024);
        }
    }

    public void AddLog(string message)
    {
        lock (_lock)
        {
            if (_recentLogs.Count >= 5)
            {
                _ = _recentLogs.Dequeue();
            }

            _recentLogs.Enqueue(message);
        }
    }

    public void MarkComplete(string message)
    {
        lock (_lock)
        {
            _isComplete = true;
            _completionMessage = message ?? string.Empty;
            _stopwatch.Stop();
        }
    }

    public CancellationToken GetCancellationToken()
    {
        lock (_lock)
        {
            return _cancellationTokenSource.Token;
        }
    }

    public void RequestCancellation()
    {
        lock (_lock)
        {
            if (_cancellationTokenSource.IsCancellationRequested is false)
            {
                _cancellationTokenSource.Cancel();
                AddLog("Cancellation requested by user");
            }
        }
    }

    public bool IsCancellationRequested
    {
        get
        {
            lock (_lock)
            {
                return _cancellationTokenSource.Token.IsCancellationRequested;
            }
        }
    }

    public void Dispose() => _cancellationTokenSource.Dispose();
}
