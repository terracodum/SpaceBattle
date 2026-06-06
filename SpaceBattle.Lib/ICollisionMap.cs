namespace StarWars.Lib;

public interface ICollisionMap
{
    bool Contains(string shapeA, string shapeB, int relX, int relY);
    void Add(string shapeA, string shapeB, int relX, int relY);
    IReadOnlyList<CollisionEntry> GetEntries();
}
