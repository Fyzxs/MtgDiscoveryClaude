using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lib.Shared.Invocation;

/// <summary>
/// Represents the execution context
/// </summary>
public interface IExecutionContext
{
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <returns>An instance of <see cref="ILogger"/></returns>
    ILogger Logger() => NullLogger.Instance;

    /// <summary>
    /// Gets the elapsed time since the execution context was created.
    /// </summary>
    /// <returns>The elapsed time.</returns>
    TimeSpan ElapsedTime();

    /// <summary>
    /// Gets the invocation ID.
    /// </summary>
    /// <returns>The invocation ID.</returns>
    string InvocationId();
}
