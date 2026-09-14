using Hwdtech;

namespace StarWars.Lib;

public class RegisterIoCDependencyCollisionDetector : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Detectors.Collision",
            (Func<object[], object>)(args =>
            {
                var map = (ICollisionMap)args[0];
                return new CollisionDetector(map);
            })).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.CheckCollision",
            (Func<object[], object>)(args =>
            {
                var order = (IDictionary<string, object>)args[0];
                return new CheckCollisionCommand(
                    (string)order["selfId"],
                    IoC.Resolve<ICollidable>("Adapters.ICollidable", order["gameObject"]),
                    (ICollidableRepository)order["repository"],
                    (ICollisionDetector)order["detector"],
                    (Hwdtech.ICommand)order["onCollision"]);
            })).Execute();
    }
}
