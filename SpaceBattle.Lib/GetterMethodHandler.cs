namespace StarWars.Lib;

public class GetterMethodHandler : IMethodHandler
{
    public bool CanHandle(string methodName) => methodName.StartsWith("get_");

    public object? Handle(
        string methodName,
        IDictionary<string, object> dictionary,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies,
        object?[]? args)
    {
        var prop = methodName[4..];
        if (strategies.TryGetValue(prop, out var strategy))
        {
            return strategy(dictionary);
        }

        return dictionary[prop];
    }
}
