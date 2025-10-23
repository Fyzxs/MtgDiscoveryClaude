using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Lib.Scryfall.Ingestion.Dashboard.RazorUI;
#pragma warning disable IDE0032 // Use auto property
internal sealed class DashboardState : IDisposable
{
    private readonly Lock _lock = new();
    private readonly Queue<string> _recentLogs = new(3);
    private readonly Dictionary<string, int> _completedCounts = [];
    private readonly Stopwatch _stopwatch = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private string _progressType = string.Empty;
    private int _progressCurrent;
    private int _progressTotal;
    private string _progressAction = string.Empty;
    private string _progressItem = string.Empty;

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
    private bool _isComplete;
    private string _completionMessage = string.Empty;

    public string ProgressType
    {
        get { lock (_lock) { return _progressType; } }
    }

    public int ProgressCurrent
    {
        get { lock (_lock) { return _progressCurrent; } }
    }

    public int ProgressTotal
    {
        get { lock (_lock) { return _progressTotal; } }
    }

    public string ProgressAction
    {
        get { lock (_lock) { return _progressAction; } }
    }

    public string ProgressItem
    {
        get { lock (_lock) { return _progressItem; } }
    }

    public int SetCurrent
    {
        get { lock (_lock) { return _setCurrent; } }
    }

    public int SetTotal
    {
        get { lock (_lock) { return _setTotal; } }
    }

    public string SetName
    {
        get { lock (_lock) { return _setName; } }
    }

    public int CardCurrent
    {
        get { lock (_lock) { return _cardCurrent; } }
    }

    public int CardTotal
    {
        get { lock (_lock) { return _cardTotal; } }
    }

    public string CardName
    {
        get { lock (_lock) { return _cardName; } }
    }

    public int RulingCurrent
    {
        get { lock (_lock) { return _rulingCurrent; } }
    }

    public int RulingTotal
    {
        get { lock (_lock) { return _rulingTotal; } }
    }

    public string RulingName
    {
        get { lock (_lock) { return _rulingName; } }
    }

    public int TrigramCurrent
    {
        get { lock (_lock) { return _trigramCurrent; } }
    }

    public int TrigramTotal
    {
        get { lock (_lock) { return _trigramTotal; } }
    }

    public string TrigramName
    {
        get { lock (_lock) { return _trigramName; } }
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

    public IReadOnlyList<string> RecentLogs
    {
        get { lock (_lock) { return _recentLogs.ToList(); } }
    }

    public IReadOnlyDictionary<string, int> CompletedCounts
    {
        get { lock (_lock) { return new Dictionary<string, int>(_completedCounts); } }
    }

    public void StartTimer()
    {
        lock (_lock)
        {
            _stopwatch.Start();
        }
    }

    public void UpdateProgress(string type, int current, int total, string action, string item)
    {
        lock (_lock)
        {
            _progressType = type ?? string.Empty;
            _progressCurrent = current;
            _progressTotal = total;
            _progressAction = action ?? string.Empty;
            _progressItem = item ?? string.Empty;
        }
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

    public void UpdateRulingProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _rulingCurrent = current;
            _rulingTotal = total;
            _rulingName = name ?? string.Empty;
        }
    }

    public void UpdateTrigramProgress(int current, int total, string name)
    {
        lock (_lock)
        {
            _trigramCurrent = current;
            _trigramTotal = total;
            _trigramName = name ?? string.Empty;
        }
    }

    public void UpdateCompletedCount(string type, int count)
    {
        lock (_lock)
        {
            _completedCounts[type] = count;
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
            if (_recentLogs.Count >= 3)
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
