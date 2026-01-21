using System;
using System.Linq;
using System.Reflection;

namespace TestConvenience.Core.Reflection;

/// <summary>
/// Creates instance of a class from a private constructor.
/// <example>
/// Usage:
/// <code>
/// ClassToInstantiate newInstance = new PrivateCtor&lt;ClassToInstantiate>, "ValueForFirstCtorArg", valueForSecondCtorArg)
/// </code>
/// </example>
/// </summary>
public sealed class PrivateCtor<T>
{
    /// <summary>
    /// Convertor to specified type T.
    /// </summary>
    /// <param name="origin"></param>
    public static implicit operator T(PrivateCtor<T> origin) => origin.Object();

    private readonly PrivateConstructorInfo _privateConstructorInfo;
    private readonly object[] _args;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivateCtor{T}"/> class.
    /// </summary>
    /// <param name="args">The arguments that will be provided to the default constructor.</param>
    public PrivateCtor(params object[] args) : this(args, new PrivateConstructorInfo(typeof(T), args)) { }

    private PrivateCtor(object[] args, PrivateConstructorInfo privateConstructorInfo)
    {
        _args = args;
        _privateConstructorInfo = privateConstructorInfo;
    }

    private T Object()
    {
        try
        {
            return (T)((ConstructorInfo)_privateConstructorInfo).Invoke(_args);
        }
        catch (NullReferenceException)
        {
            throw new AssertionFailedException($"Unable to find a constructor for {_privateConstructorInfo.ExceptionInfo()}");
        }
    }
}

/// <summary>
/// <see cref="PrivateConstructorInfo"/> gets the <see cref="ConstructorInfo"/> for the private constructor matching the arguments.
/// </summary>
internal sealed class PrivateConstructorInfo
{
    private readonly Type _type;
    private readonly Type[] _types;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivateConstructorInfo"/> class.
    /// </summary>
    /// <param name="type">The type of to instantiate</param>
    /// <param name="args">The arguments matching the private constructor to invoke.c</param>
    public PrivateConstructorInfo(Type type, object[] args) : this(type, new ValueTypeArray(args)) { }

    private PrivateConstructorInfo(Type type, Type[] types)
    {
        _type = type;
        _types = types;
    }

    /// <summary>
    /// Conversion to <see cref="ConstructorInfo"/>.
    /// </summary>
    /// <param name="origin"></param>
    public static implicit operator ConstructorInfo(PrivateConstructorInfo origin) => origin.CtorInfo();

    private ConstructorInfo CtorInfo() => _type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, _types, null);

    public string ExceptionInfo() => $"{_type.Name} with the parameters {GetArgsString()}";

    private string GetArgsString() => string.Join(", ", _types.Select(t => t.Name));
}

/// <summary>
/// Transforms a collection of object into an array of the instance <see cref="Type"/>.
/// </summary>
internal sealed class ValueTypeArray : Array<Type>
{
    private readonly object[] _args;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueTypeArray"/> class.
    /// </summary>
    /// <param name="args">The Objects</param>
    public ValueTypeArray(params object[] args) => _args = args;

    /// <inheritdoc/>
    protected override Type[] Value()
    {
        Type[] types = new Type[_args.Length];
        for (int index = 0; index < _args.Length; index++)
        {
            types[index] = _args[index].GetType();
        }

        return types;
    }
}
internal abstract class Array<T>
{
    /// <summary>
    /// Implicit conversion from the object to an array of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="origin"></param>
    public static implicit operator T[](Array<T> origin) => origin.Value();

    /// <summary>
    /// Overriding class provides an array of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>An array of type <typeparamref name="T"/></returns>
    protected abstract T[] Value();
}
