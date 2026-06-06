namespace StarWars.Lib;

public interface IMethodHandler
{
    bool CanHandle(string methodName);

    object? Handle(
        string methodName,
        IDictionary<string, object> dictionary,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies,
        object?[]? args);
}
