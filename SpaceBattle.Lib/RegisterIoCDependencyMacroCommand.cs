using Hwdtech;

namespace StarWars.Lib;

public class RegisterIoCDependencyMacroCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Macro",
            (Func<object[], object>)(args => new MacroCommand((IEnumerable<Hwdtech.ICommand>)args[0]))).Execute();
    }
}
