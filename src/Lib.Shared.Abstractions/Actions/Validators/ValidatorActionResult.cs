using System;

namespace Lib.Shared.Abstractions.Actions.Validators;

public sealed class ValidatorActionResult<TFailureStatus> : IValidatorActionResult<TFailureStatus> where TFailureStatus : class
{
    private readonly TFailureStatus _failureStatus;
    private readonly bool _isValid;

    public ValidatorActionResult(TFailureStatus failureStatus) : this(false, failureStatus) { }

    public ValidatorActionResult() : this(true, null) { }

    private ValidatorActionResult(bool isValid, TFailureStatus failureStatus)
    {
        _isValid = isValid;
        _failureStatus = failureStatus;
    }

    public bool IsValid() => _isValid;

    public TFailureStatus FailureStatus()
    {
        if (IsValid()) throw new InvalidOperationException("Cannot retrieve failureStatus of Valid result.");

        return _failureStatus;
    }
}
