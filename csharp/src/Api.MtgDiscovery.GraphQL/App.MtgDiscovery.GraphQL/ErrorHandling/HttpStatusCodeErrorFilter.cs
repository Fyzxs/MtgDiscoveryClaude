using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HotChocolate;
using Microsoft.Extensions.Hosting;

namespace App.MtgDiscovery.GraphQL.ErrorHandling;

internal sealed class HttpStatusCodeErrorFilter : IErrorFilter
{
    private readonly IHostEnvironment _environment;

    public HttpStatusCodeErrorFilter(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public IError OnError([NotNull] IError error)
    {
        Dictionary<string, object> extensions = new Dictionary<string, object>
        {
            ["timestamp"] = System.DateTime.UtcNow.ToString("O")
        };

        if (error.Code == "AUTH_NOT_AUTHENTICATED" || error.Code == "AUTH_NOT_AUTHORIZED")
        {
            extensions["statusCode"] = 401;
        }

        // In development, include more debugging information
        if (_environment.IsDevelopment())
        {
            extensions["path"] = error.Path?.ToList();
            extensions["locations"] = error.Locations?.ToList();

            if (error.Exception != null)
            {
                extensions["exceptionType"] = error.Exception.GetType().Name;
                extensions["exceptionMessage"] = error.Exception.Message;

                // Include full stack trace in development
                if (error.Exception.InnerException != null)
                {
                    extensions["innerException"] = error.Exception.InnerException.Message;
                    extensions["innerExceptionType"] = error.Exception.InnerException.GetType().Name;
                }
            }
        }

        return error.WithExtensions(extensions);
    }
}
