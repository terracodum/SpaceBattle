using System.Reflection;

namespace StarWars.Lib;

public class DictionaryAdapter<T> : DispatchProxy where T : class
{
    private IDictionary<string, object> _dictionary = null!;
    private IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> _strategies = null!;
    private IReadOnlyList<IMethodHandler> _handlers = null!;

    public static T Create(
        IDictionary<string, object> dictionary,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies,
        IReadOnlyList<IMethodHandler> handlers)
    {
        var proxy = Create<T, DictionaryAdapter<T>>() as DictionaryAdapter<T>;
        proxy!._dictionary = dictionary;
        proxy._strategies = strategies;
        proxy._handlers = handlers;
        return (T)(object)proxy;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var name = targetMethod!.Name;
        var handler = _handlers.FirstOrDefault(h => h.CanHandle(name));
        if (handler is not null)
        {
            return handler.Handle(name, _dictionary, _strategies, args);
        }

        throw new NotSupportedException($"No handler registered for method '{name}'.");
    }
}
