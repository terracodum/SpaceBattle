namespace StarWars.Lib;

public class CollisionMap : ICollisionMap
{
    private readonly HashSet<CollisionEntry> _data = new();

    public bool Contains(string shapeA, string shapeB, int relX, int relY)
        => _data.Contains(new CollisionEntry(shapeA, shapeB, relX, relY))
        || _data.Contains(new CollisionEntry(shapeB, shapeA, -relX, -relY));

    public void Add(string shapeA, string shapeB, int relX, int relY)
        => _data.Add(new CollisionEntry(shapeA, shapeB, relX, relY));

    public IReadOnlyList<CollisionEntry> GetEntries() => _data.ToList();
}
