using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HotChocolate;

namespace App.MtgDiscovery.GraphQL.ErrorHandling;

public class HttpStatusCodeErrorFilter : IErrorFilter
{
    public IError OnError([NotNull] IError error)
    {
        if (error.Code == "AUTH_NOT_AUTHENTICATED" || error.Code == "AUTH_NOT_AUTHORIZED")
        {
            return error.WithExtensions(new Dictionary<string, object>
            {
                ["statusCode"] = 401
            });
        }

        return error;
    }
}
