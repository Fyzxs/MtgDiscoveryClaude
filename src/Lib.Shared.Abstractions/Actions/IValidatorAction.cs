using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions;

public interface IValidatorAction<in TItem, TFailureStatus>
{
    Task<IValidatorActionResult<TFailureStatus>> Validate(TItem item);
}