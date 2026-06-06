namespace StarWars.Lib;

public interface ICollidable
{
    Vector GetPosition();
    Vector GetVelocity();
    string GetShapeId();
}
