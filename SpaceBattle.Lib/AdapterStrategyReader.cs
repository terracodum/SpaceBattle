using System.Reflection;

namespace StarWars.Lib;

public class AdapterStrategyReader<T> where T : class
{
    private readonly Assembly _assembly;

    public AdapterStrategyReader(Assembly assembly)
    {
        _assembly = assembly;
    }

    public IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> Read()
        => _assembly
            .GetCustomAttributes<AdapterAttribute>()
            .Where(a => a.InterfaceType == typeof(T))
            .ToDictionary(
                a => a.PropertyName,
                a => (Func<IDictionary<string, object>, object>)
                    a.ResolverType
                        .GetMethod("Resolve", BindingFlags.Public | BindingFlags.Static)!
                        .CreateDelegate(typeof(Func<IDictionary<string, object>, object>))
            );
}
