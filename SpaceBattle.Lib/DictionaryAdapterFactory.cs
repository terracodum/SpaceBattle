namespace StarWars.Lib;

public class DictionaryAdapterFactory<T> : IAdapterFactory<T> where T : class
{
    private readonly IReadOnlyList<IMethodHandler> _handlers;

    public DictionaryAdapterFactory(IReadOnlyList<IMethodHandler> handlers)
    {
        _handlers = handlers;
    }

    public T Create(
        IDictionary<string, object> dictionary,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies)
        => DictionaryAdapter<T>.Create(dictionary, strategies, _handlers);
}
