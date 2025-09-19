using System.Threading.Tasks;
using Lib.Universal.Extensions;

namespace Lib.Shared.Abstractions.Actions.Validators;

public interface IValidatorActionResponse<in TToValidate, TReturnResponse>
{
    Task<IAsyncTryOut<TReturnResponse>> TryIsValid(TToValidate toValidate);
}
