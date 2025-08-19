using System.Threading.Tasks;

namespace Lib.Shared.Invocation;

/// <summary>
/// Represents the execution context of an Authenticated Call
/// </summary>
public interface IAuthNExecutionContext : IExecutionContext
{
    /// <summary>
    /// Information about the source of the caller triggering the execution.
    /// </summary>
    /// <returns></returns>
    Task<ICaller> Caller();
}