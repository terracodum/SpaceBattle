using Hwdtech;

namespace StarWars.Lib;

public class RegisterIoCDependencyMoveCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Move",
            (Func<object, object>)(obj =>
            new MoveCommand(IoC.Resolve<IMovingObject>("Adapters.IMovingObject", obj)))).Execute();
    }
}
