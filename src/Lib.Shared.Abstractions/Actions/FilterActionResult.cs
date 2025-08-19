using System;

namespace Lib.Shared.Abstractions.Actions;

public sealed class FilterActionResult<TFailureStatus> : IFilterActionResult<TFailureStatus> where TFailureStatus : class
{
    private readonly TFailureStatus _failureStatus;
    private readonly bool _isFilteredOut;

    public FilterActionResult(TFailureStatus failureStatus) : this(true, failureStatus) { }

    public FilterActionResult() : this(false, null) { }

    private FilterActionResult(bool isFilteredOut, TFailureStatus failureStatus)
    {
        _isFilteredOut = isFilteredOut;
        _failureStatus = failureStatus;
    }

    public bool IsFilteredOut() => _isFilteredOut;

    public TFailureStatus FailureStatus()
    {
        if (((IFilterActionResult<TFailureStatus>)this).IsNotFilteredOut()) throw new InvalidOperationException("Cannot retrieve failureStatus of non-filtered result.");

        return _failureStatus;
    }
}