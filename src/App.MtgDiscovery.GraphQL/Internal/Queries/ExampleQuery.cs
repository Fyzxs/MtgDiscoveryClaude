using System;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Internal.Queries;

public class ExampleQuery;

[ExtendObjectType(typeof(ExampleQuery))]
public class ExampleQueryMethods
{
    public string Hello() => "Hello World from GraphQL!";

    public string Greeting(string name) => $"Hello, {name}!";

    public DateTime CurrentTime() => DateTime.UtcNow;
}
