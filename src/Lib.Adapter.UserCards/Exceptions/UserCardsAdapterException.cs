using System;
using System.Net;
using Lib.Shared.Invocation.Exceptions;

namespace Lib.Adapter.UserCards.Exceptions;

/// <summary>
/// Exception thrown when UserCards adapter operations fail.
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
public sealed class UserCardsAdapterException : OperationException
{
    public UserCardsAdapterException() : base(HttpStatusCode.InternalServerError, "UserCards adapter operation failed")
    {
    }

    public UserCardsAdapterException(string message) : base(HttpStatusCode.InternalServerError, message)
    {
    }

    public UserCardsAdapterException(string message, Exception innerException) : base(HttpStatusCode.InternalServerError, message, innerException)
    {
    }
}