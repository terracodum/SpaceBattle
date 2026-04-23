namespace StarWars.Lib;

public interface IMovingObject
{
    Vector GetPosition();
    Vector GetVelocity();
    void SetPosition(Vector position);
}

public class MoveCommand : ICommand
{
    private readonly IMovingObject obj;

    public MoveCommand(IMovingObject obj)
    {
        this.obj = obj;
    }

    public void Execute()
    {
        obj.SetPosition(obj.GetPosition() + obj.GetVelocity());
    }
}
