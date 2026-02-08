using Lib.Shared.Abstractions.Actions.Validators;

namespace Lib.MtgDiscovery.Entry.Tests.Fakes;

internal sealed class ValidatorActionResultFake<TFailureStatus> : IValidatorActionResult<TFailureStatus>
{
    private readonly bool _isValid;
    private readonly TFailureStatus _failureStatus;

    public ValidatorActionResultFake(bool isValid, TFailureStatus failureStatus = default!)
    {
        _isValid = isValid;
        _failureStatus = failureStatus;
    }

    public bool IsValid() => _isValid;
    public TFailureStatus FailureStatus() => _failureStatus;
}
