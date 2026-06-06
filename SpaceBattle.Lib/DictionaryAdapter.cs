using System.Reflection;

namespace StarWars.Lib;

public class DictionaryAdapter<T> : DispatchProxy where T : class
{
    private IDictionary<string, object> _dictionary = null!;
    private IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> _strategies = null!;

    public static T Create(
        IDictionary<string, object> dictionary,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies)
    {
        var proxy = Create<T, DictionaryAdapter<T>>() as DictionaryAdapter<T>;
        proxy!._dictionary = dictionary;
        proxy._strategies = strategies;
        return (T)(object)proxy;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var name = targetMethod!.Name;

        if (name.StartsWith("get_"))
        {
            var prop = name[4..];
            if (_strategies.TryGetValue(prop, out var strategy))
            {
                return strategy(_dictionary);
            }

            return _dictionary[prop];
        }

        if (name.StartsWith("set_"))
        {
            _dictionary[name[4..]] = args![0]!;
            return null;
        }

        throw new NotSupportedException($"Method '{name}' is not supported by DictionaryAdapter.");
    }
}
