using System.Reflection;
using Hwdtech;

namespace StarWars.Lib;

public class RegisterAdapterDependency<T> : ICommand where T : class
{
    private readonly Assembly _assembly;

    public RegisterAdapterDependency(Assembly assembly)
    {
        _assembly = assembly;
    }

    public void Execute()
    {
        var strategies = _assembly
            .GetCustomAttributes<AdapterAttribute>()
            .Where(a => a.InterfaceType == typeof(T))
            .ToDictionary(
                a => a.PropertyName,
                a => (Func<IDictionary<string, object>, object>)
                    a.ResolverType
                        .GetMethod("Resolve", BindingFlags.Public | BindingFlags.Static)!
                        .CreateDelegate(typeof(Func<IDictionary<string, object>, object>))
            );

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            $"Adapters.{typeof(T).Name}",
            (Func<object, object>)(obj =>
                DictionaryAdapter<T>.Create(
                    (IDictionary<string, object>)((object[])obj)[0],
                    strategies
                )
            )
        ).Execute();
    }
}
