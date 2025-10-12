using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserSetCards.Exceptions;

/// <summary>
/// Exception thrown when UserSetCards adapter operations fail.
/// Follows the established adapter exception pattern from other adapters.
///
/// Design Pattern: Adapter-specific exception
/// Extends OperationException to maintain consistency with the operation response framework.
/// All adapter-layer exceptions should derive from OperationException to ensure
/// proper integration with the error handling pipeline.
///
/// Usage Pattern:
/// - Wrap all external dependencies (Cosmos DB, network calls, etc.)
/// - Provide meaningful error context for troubleshooting
/// - Include original exception for full error chain
/// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class UserSetCardsAdapterException : OperationException
#pragma warning restore CA1032
{
    public UserSetCardsAdapterException() : base(HttpStatusCode.InternalServerError, "UserSetCards adapter operation failed")
    {
    }
}
