using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Actions.Validators;

public interface IValidator<in TItem>
{
    Task<bool> IsValid(TItem entity);
    async Task<bool> IsNotValid(TItem item) => (await IsValid(item).ConfigureAwait(false)) is false;
}
