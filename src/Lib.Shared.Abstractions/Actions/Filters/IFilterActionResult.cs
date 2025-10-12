namespace Lib.Shared.Abstractions.Actions.Filters;

public interface IFilterActionResult<out TFailureStatus>
{
    bool IsFilteredOut();
    bool IsNotFilteredOut() => IsFilteredOut() is false;
    TFailureStatus FailureStatus();
}
