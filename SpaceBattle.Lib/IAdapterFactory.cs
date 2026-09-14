namespace StarWars.Lib;

public interface IAdapterFactory<T> where T : class
{
    T Create(
        IDictionary<string, object> dictionary,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies);
}
