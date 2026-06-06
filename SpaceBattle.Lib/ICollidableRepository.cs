namespace StarWars.Lib;

public interface ICollidableRepository
{
    IEnumerable<(string Id, ICollidable Collidable)> GetAll();
}
