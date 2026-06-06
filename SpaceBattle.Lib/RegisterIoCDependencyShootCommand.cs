using Hwdtech;

namespace StarWars.Lib;

public class RegisterIoCDependencyShootCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Shoot",
            (Func<object[], object>)(args =>
            {
                var order = (IDictionary<string, object>)args[0];
                return new ShootCommand(
                    IoC.Resolve<IShootable>("Adapters.IShootable", order["gameObject"]),
                    (IGameObjectRepository)order["repository"],
                    (ICommandReceiver)order["queue"]);
            })).Execute();
    }
}
