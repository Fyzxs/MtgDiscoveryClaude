using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lib.Universal.Inversion;

public interface IServiceLocator
{
    /// <summary>Gets the service object of the specified type.</summary>
    /// <typeparam name="TRegisterType">The type used to retrieve the object</typeparam>
    /// <typeparam name="TReturnType">The type of the object</typeparam>
    /// <returns>A service object of type <typeparamref name="TRegisterType" />.</returns>
    /// 
    /// --or--
    /// 
    /// Throws <see cref="ArgumentException"/> if there is no service register of type <typeparamref name="TRegisterType" />.
    TReturnType GetService<TRegisterType, TReturnType>();
    /// <summary>Gets the service object of the specified type.</summary>
    /// <typeparam name="TServiceType">The type used to retrieve the object</typeparam>
    /// <typeparam name="TReturnType">The type of the object</typeparam>
    /// <returns>A service object of type <typeparamref name="TServiceType" />.</returns>
    /// 
    /// --or--
    /// 
    /// Throws <see cref="ArgumentException"/> if there is no service object of type <typeparamref name="TServiceType" />.
    TServiceType GetService<TServiceType>();
}

public sealed class ServiceLocator : IServiceLocator
{
    private static readonly ConcurrentDictionary<Type, object> s_cached = new();
    private static readonly Dictionary<Type, Func<object>> s_factories = [];

    public static ServiceLocator Instance { get; } = new ServiceLocator();

    private ServiceLocator() { }

    public static void ServiceRegister<TRegisterType>(Func<object> serviceFactory)
    {
        Type type = typeof(TRegisterType);
        s_factories.TryAdd(type, serviceFactory);
    }

    public TReturnType GetService<TRegisterType, TReturnType>()
    {
        Type type = typeof(TRegisterType);
        bool factoryExists = s_factories.TryGetValue(type, out Func<object> factory);
        if (factoryExists is false) throw new ArgumentException($"Service not registered [name={type.Name}].");

        object orAdd = s_cached.GetOrAdd(type, (_) => factory!());
        return (TReturnType)orAdd;
    }

    public TServiceType GetService<TServiceType>() => GetService<TServiceType, TServiceType>();
}
