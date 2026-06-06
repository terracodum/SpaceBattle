using Hwdtech;

namespace StarWars.Lib;

public class RegisterIoCDependencyMacroCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Macro",
            (Func<object, object>)(obj => new MacroCommand((IEnumerable<Hwdtech.ICommand>)obj))).Execute();
    }
}
