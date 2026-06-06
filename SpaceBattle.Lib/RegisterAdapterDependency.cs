using Hwdtech;

namespace StarWars.Lib;

public class RegisterAdapterDependency<T> : ICommand where T : class
{
    private readonly IAdapterFactory<T> _factory;
    private readonly IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> _strategies;

    public RegisterAdapterDependency(
        IAdapterFactory<T> factory,
        IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> strategies)
    {
        _factory = factory;
        _strategies = strategies;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            $"Adapters.{typeof(T).Name}",
            (Func<object, object>)(obj =>
                _factory.Create(
                    (IDictionary<string, object>)((object[])obj)[0],
                    _strategies
                )
            )
        ).Execute();
    }
}
