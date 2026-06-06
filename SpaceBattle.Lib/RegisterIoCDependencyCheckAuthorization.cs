using Hwdtech;

namespace StarWars.Lib;

public class RegisterIoCDependencyCheckAuthorization : Hwdtech.ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.CheckAuthorization",
            (Func<object[], object>)(args =>
            {
                var order = (IDictionary<string, object>)args[0];
                return new CheckAuthorizationCommand(
                    (IAuthorizationService)order["authService"],
                    (string)order["gameId"],
                    (string)order["objectId"],
                    (string)order["playerId"]);
            })).Execute();
    }
}
