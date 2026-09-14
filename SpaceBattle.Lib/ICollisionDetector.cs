namespace StarWars.Lib;

public interface ICollisionDetector
{
    bool Collides(ICollidable a, ICollidable b);
}
