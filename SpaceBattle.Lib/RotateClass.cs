namespace StarWars.Lib;

public interface IRotatingObject
{
    Angle GetAngle();
    void SetAngle(Angle angle);
    Angle GetAngularVelocity();
}

public class RotateCommand : ICommand
{
    private readonly IRotatingObject obj;

    public RotateCommand(IRotatingObject obj)
    {
        this.obj = obj;
    }

    public void Execute()
    {
        obj.SetAngle(obj.GetAngle() + obj.GetAngularVelocity());
    }
}
