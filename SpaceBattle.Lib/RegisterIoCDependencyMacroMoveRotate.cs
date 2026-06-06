using Hwdtech;

namespace StarWars.Lib;

public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Macro.Move",
            (Func<object[], object>)(args => new CreateMacroCommandStrategy("Move").Resolve(args))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Macro.Rotate",
            (Func<object[], object>)(args => new CreateMacroCommandStrategy("Rotate").Resolve(args))).Execute();
    }
}
