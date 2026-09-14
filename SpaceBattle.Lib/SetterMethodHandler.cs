namespace StarWars.Lib;

public class SetterMethodHandler : IMethodHandler
{
    public bool CanHandle(string methodName) => methodName.StartsWith("set_");

    public object? Handle(
        string methodName,
        IDictionary<string, object> dictionary,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies,
        object?[]? args)
    {
        dictionary[methodName[4..]] = args![0]!;
        return null;
    }
}
