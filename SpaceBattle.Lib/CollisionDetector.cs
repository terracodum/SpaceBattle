namespace StarWars.Lib;

// Handles the discrete-world case: collision is detected if objects overlap
// at the current tick OR at the next tick (after velocity is applied).
// This catches objects that would pass through each other between frames.
public class CollisionDetector : ICollisionDetector
{
    private readonly ICollisionMap _map;

    public CollisionDetector(ICollisionMap map)
    {
        _map = map;
    }

    public bool Collides(ICollidable a, ICollidable b)
        => CollidesAt(a.GetShapeId(), b.GetShapeId(), a.GetPosition(), b.GetPosition())
        || CollidesAt(a.GetShapeId(), b.GetShapeId(),
                a.GetPosition() + a.GetVelocity(),
                b.GetPosition() + b.GetVelocity());

    private bool CollidesAt(string shapeA, string shapeB, Vector posA, Vector posB)
    {
        var elA = posA.GetElements();
        var elB = posB.GetElements();
        return _map.Contains(shapeA, shapeB, elB[0] - elA[0], elB[1] - elA[1]);
    }
}
