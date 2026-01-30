using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Cli.MtgDiscovery.PriceUpdate.Dashboard.RazorUI;

#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0044 // Make field readonly
internal sealed class PriceUpdateDashboardState : IDisposable
{
    private readonly Lock _lock = new();
    private readonly Queue<string> _recentLogs = new(5);
    private readonly Stopwatch _stopwatch = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private string _currentContainer = string.Empty;
    private int _containerIndex;
    private int _totalContainers = 4;

    private int _current;
    private int _total;
    private string _currentCard = string.Empty;

    private int _updatedCount;
    private int _skippedCount;
    private int _errorCount;
    private double _totalRu;

    private long _memoryUsage;
    private bool _isComplete;
    private string _completionMessage = string.Empty;
    private string _status = "Initializing...";

    public string CurrentContainer
    {
        get { lock (_lock) { return _currentContainer; } }
    }

    public int ContainerIndex
    {
        get { lock (_lock) { return _containerIndex; } }
    }

    public int TotalContainers
    {
        get { lock (_lock) { return _totalContainers; } }
    }

    public int Current
    {
        get { lock (_lock) { return _current; } }
    }

    public int Total
    {
        get { lock (_lock) { return _total; } }
    }

    public string CurrentCard
    {
        get { lock (_lock) { return _currentCard; } }
    }

    public int UpdatedCount
    {
        get { lock (_lock) { return _updatedCount; } }
    }

    public int SkippedCount
    {
        get { lock (_lock) { return _skippedCount; } }
    }

    public int ErrorCount
    {
        get { lock (_lock) { return _errorCount; } }
    }

    public double TotalRu
    {
        get { lock (_lock) { return _totalRu; } }
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

    public void SetContainer(string containerName, int containerIndex, int total)
    {
        lock (_lock)
        {
            _currentContainer = containerName ?? string.Empty;
            _containerIndex = containerIndex;
            _total = total;
            _current = 0;
        }
    }

    public void UpdateProgress(int current, string cardName)
    {
        lock (_lock)
        {
            _current = current;
            _currentCard = cardName ?? string.Empty;
        }
    }

    public void IncrementUpdated()
    {
        lock (_lock)
        {
            _updatedCount++;
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

    public void AddRu(double ru)
    {
        lock (_lock)
        {
            _totalRu += ru;
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
