using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Cli.MtgDiscovery.UserDataReconciler.Dashboard.RazorUI;

#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0044 // Make field readonly
internal sealed class ReconcilerDashboardState : IDisposable
{
    private readonly Lock _lock = new();
    private readonly Queue<string> _recentLogs = new(8);
    private readonly Stopwatch _stopwatch = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private bool _isDryRun = true;
    private string _status = "Initializing...";
    private int _totalUsers;
    private int _currentUserIndex;
    private string _currentUserId = string.Empty;
    private string _currentSetCode = string.Empty;
    private int _userSetsProcessed;
    private int _userTotalSets;

    private int _totalSetsProcessed;
    private int _setsWithDiscrepancy;
    private int _setsUpdated;
    private int _cardsAdded;
    private int _cardsRemoved;
    private int _errorCount;

    private long _memoryUsage;
    private bool _isComplete;
    private string _completionMessage = string.Empty;

    public bool IsDryRun
    {
        get { lock (_lock) { return _isDryRun; } }
    }

    public string Status
    {
        get { lock (_lock) { return _status; } }
    }

    public int TotalUsers
    {
        get { lock (_lock) { return _totalUsers; } }
    }

    public int CurrentUserIndex
    {
        get { lock (_lock) { return _currentUserIndex; } }
    }

    public string CurrentUserId
    {
        get { lock (_lock) { return _currentUserId; } }
    }

    public string CurrentSetCode
    {
        get { lock (_lock) { return _currentSetCode; } }
    }

    public int UserSetsProcessed
    {
        get { lock (_lock) { return _userSetsProcessed; } }
    }

    public int UserTotalSets
    {
        get { lock (_lock) { return _userTotalSets; } }
    }

    public int TotalSetsProcessed
    {
        get { lock (_lock) { return _totalSetsProcessed; } }
    }

    public int SetsWithDiscrepancy
    {
        get { lock (_lock) { return _setsWithDiscrepancy; } }
    }

    public int SetsUpdated
    {
        get { lock (_lock) { return _setsUpdated; } }
    }

    public int CardsAdded
    {
        get { lock (_lock) { return _cardsAdded; } }
    }

    public int CardsRemoved
    {
        get { lock (_lock) { return _cardsRemoved; } }
    }

    public int ErrorCount
    {
        get { lock (_lock) { return _errorCount; } }
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
        get { lock (_lock) { return [.. _recentLogs]; } }
    }

    public void StartTimer()
    {
        lock (_lock)
        {
            _stopwatch.Start();
        }
    }

    public void SetMode(bool isDryRun)
    {
        lock (_lock)
        {
            _isDryRun = isDryRun;
        }
    }

    public void SetStatus(string status)
    {
        lock (_lock)
        {
            _status = status ?? string.Empty;
        }
    }

    public void SetTotalUsers(int count)
    {
        lock (_lock)
        {
            _totalUsers = count;
        }
    }

    public void StartUser(string userId, int userIndex)
    {
        lock (_lock)
        {
            _currentUserId = userId ?? string.Empty;
            _currentUserIndex = userIndex;
            _userSetsProcessed = 0;
            _userTotalSets = 0;
            _currentSetCode = string.Empty;
        }
    }

    public void UpdateUserProgress(string setCode, int setsProcessed, int totalSets)
    {
        lock (_lock)
        {
            _currentSetCode = setCode ?? string.Empty;
            _userSetsProcessed = setsProcessed;
            _userTotalSets = totalSets;
        }
    }

    public void IncrementSetsProcessed()
    {
        lock (_lock)
        {
            _totalSetsProcessed++;
        }
    }

    public void IncrementSetsWithDiscrepancy()
    {
        lock (_lock)
        {
            _setsWithDiscrepancy++;
        }
    }

    public void IncrementSetsUpdated()
    {
        lock (_lock)
        {
            _setsUpdated++;
        }
    }

    public void AddCardsAdded(int count)
    {
        lock (_lock)
        {
            _cardsAdded += count;
        }
    }

    public void AddCardsRemoved(int count)
    {
        lock (_lock)
        {
            _cardsRemoved += count;
        }
    }

    public void IncrementError()
    {
        lock (_lock)
        {
            _errorCount++;
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
            if (_recentLogs.Count >= 8)
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
