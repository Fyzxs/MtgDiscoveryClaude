namespace Lib.Shared.Abstractions.Actions.Validators;

public interface IValidatorActionResult<out TFailureStatus>
{
    bool IsValid();
    bool IsNotValid() => IsValid() is false;
    TFailureStatus FailureStatus();
}
