namespace StarWars.Lib;

// Analytical swept-segment collision detector.
// For each cell of A, checks if its path over one tick (a line segment)
// intersects any cell of B using the slab method.
// Complexity: O(|cellsA| × |cellsB|) — constant for fixed shapes.
// No tunneling regardless of velocity magnitude.
public class SweptCollisionDetector : ICollisionDetector
{
    private readonly IShapeProvider _shapes;

    public SweptCollisionDetector(IShapeProvider shapes)
    {
        _shapes = shapes;
    }

    public bool Collides(ICollidable a, ICollidable b)
    {
        var shapeA = _shapes.GetShape(a.GetShapeId());
        var shapeB = _shapes.GetShape(b.GetShapeId());

        var posA = a.GetPosition().GetElements();
        var posB = b.GetPosition().GetElements();
        var velA = a.GetVelocity().GetElements();
        var velB = b.GetVelocity().GetElements();

        var dvX = (float)(velA[0] - velB[0]);
        var dvY = (float)(velA[1] - velB[1]);

        foreach (var cellA in shapeA.Cells)
        {
            var startX = (float)(posA[0] + cellA.X - posB[0]);
            var startY = (float)(posA[1] + cellA.Y - posB[1]);

            foreach (var cellB in shapeB.Cells)
            {
                if (SegmentHitsCell(startX, startY, startX + dvX, startY + dvY, cellB.X, cellB.Y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool SegmentHitsCell(float sx, float sy, float ex, float ey, int cx, int cy)
    {
        var (txEnter, txExit) = SlabInterval(sx, ex - sx, cx - 0.5f, cx + 0.5f);
        var (tyEnter, tyExit) = SlabInterval(sy, ey - sy, cy - 0.5f, cy + 0.5f);

        var tEnter = Math.Max(txEnter, tyEnter);
        var tExit = Math.Min(txExit, tyExit);

        return tEnter <= tExit && tExit >= 0f && tEnter <= 1f;
    }

    private static (float Enter, float Exit) SlabInterval(float s, float d, float min, float max)
    {
        if (Math.Abs(d) < 1e-9f)
        {
            return s >= min && s <= max
                ? (float.NegativeInfinity, float.PositiveInfinity)
                : (float.PositiveInfinity, float.NegativeInfinity);
        }

        var t1 = (min - s) / d;
        var t2 = (max - s) / d;
        return t1 < t2 ? (t1, t2) : (t2, t1);
    }
}
