namespace Lib.Shared.Abstractions.Actions;

public interface IFilterActionResult<out TFailureStatus>
{
    bool IsFilteredOut();
    bool IsNotFilteredOut() => IsFilteredOut() is false;
    TFailureStatus FailureStatus();
}