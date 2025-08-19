namespace Lib.Shared.Abstractions.Actions;

public interface IValidatorActionResult<out TFailureStatus>
{
    bool IsValid();
    bool IsNotValid() => IsValid() is false;
    TFailureStatus FailureStatus();
}