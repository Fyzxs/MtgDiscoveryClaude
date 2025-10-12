using System.Threading.Tasks;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Actions;

/// <summary>
/// Validator that returns both validation result and potential failure response.
/// </summary>
internal interface IValidatorWithResponse<in TToValidate, TResponseType>
{
    /// <summary>
    /// Validates input and returns failure response if invalid.
    /// </summary>
    Task<AsyncTryOut<IOperationResponse<TResponseType>>> TryValidate(TToValidate toValidate);
}