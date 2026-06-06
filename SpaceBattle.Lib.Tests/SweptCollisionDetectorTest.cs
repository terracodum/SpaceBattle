using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class SweptCollisionDetectorTest
{
    private static readonly IShapeFootprint PointShape = new ShapeFootprint("point", [(0, 0)]);
    private static readonly IShapeFootprint WideShape = new ShapeFootprint("wide", [(0, 0), (1, 0), (2, 0)]);

    private static ICollisionDetector MakeDetector(params IShapeFootprint[] shapes)
    {
        var provider = new ShapeProvider();
        foreach (var s in shapes)
        {
            provider.Register(s);
        }

        return new SweptCollisionDetector(provider);
    }

    private static Mock<ICollidable> MakeCollidable(int x, int y, int vx, int vy, string shapeId)
    {
        var mock = new Mock<ICollidable>();
        mock.Setup(c => c.GetPosition()).Returns(new Vector([x, y]));
        mock.Setup(c => c.GetVelocity()).Returns(new Vector([vx, vy]));
        mock.Setup(c => c.GetShapeId()).Returns(shapeId);
        return mock;
    }

    [Fact]
    public void Collides_SamePosition_ReturnsTrue()
    {
        var detector = MakeDetector(PointShape);

        var a = MakeCollidable(5, 5, 0, 0, "point");
        var b = MakeCollidable(5, 5, 0, 0, "point");

        Assert.True(detector.Collides(a.Object, b.Object));
    }

    [Fact]
    public void Collides_FarApart_ReturnsFalse()
    {
        var detector = MakeDetector(PointShape);

        var a = MakeCollidable(0, 0, 0, 0, "point");
        var b = MakeCollidable(10, 0, 0, 0, "point");

        Assert.False(detector.Collides(a.Object, b.Object));
    }

    [Fact]
    public void Collides_FastTorpedoPassesThroughShip_ReturnsTrue()
    {
        // Torpedo at (2,3), velocity (10,0) → jumps to (12,3) in one tick.
        // Ship at (5,3), stationary.
        // Old two-tick check would MISS this. Swept test catches it.
        var detector = MakeDetector(PointShape);

        var torpedo = MakeCollidable(2, 3, 10, 0, "point");
        var ship = MakeCollidable(5, 3, 0, 0, "point");

        Assert.True(detector.Collides(torpedo.Object, ship.Object));
    }

    [Fact]
    public void Collides_FastTorpedoMissesShip_ReturnsFalse()
    {
        // Torpedo at (2,5), velocity (10,0) → path along y=5.
        // Ship at (5,3) — different row, no collision.
        var detector = MakeDetector(PointShape);

        var torpedo = MakeCollidable(2, 5, 10, 0, "point");
        var ship = MakeCollidable(5, 3, 0, 0, "point");

        Assert.False(detector.Collides(torpedo.Object, ship.Object));
    }

    [Fact]
    public void Collides_TorpedoHitsWideShipInMiddle_ReturnsTrue()
    {
        // Ship occupies cells (5,3),(6,3),(7,3). Torpedo path passes through (6,3).
        var detector = MakeDetector(PointShape, WideShape);

        var torpedo = MakeCollidable(2, 3, 20, 0, "point");
        var ship = MakeCollidable(5, 3, 0, 0, "wide");

        Assert.True(detector.Collides(torpedo.Object, ship.Object));
    }

    [Fact]
    public void Collides_BothMoving_RelativeMotionConsidered()
    {
        // A at (0,0) vel (5,0). B at (8,0) vel (2,0).
        // Relative velocity of A to B = (3,0).
        // Relative start = (0-8)=-8. Relative end = -8+3=-5.
        // B's cell (0,0): occupies [-0.5, 0.5]. Segment [-8,-5] doesn't reach → no collision.
        var detector = MakeDetector(PointShape);

        var a = MakeCollidable(0, 0, 5, 0, "point");
        var b = MakeCollidable(8, 0, 2, 0, "point");

        Assert.False(detector.Collides(a.Object, b.Object));
    }

    [Fact]
    public void Collides_BothMovingHeadOn_ReturnsTrue()
    {
        // A at (0,0) vel (4,0). B at (6,0) vel (-4,0).
        // Relative vel = (8,0). Relative start = -6. Relative end = -6+8 = 2.
        // B's cell (0,0): segment from -6 to 2 passes through 0 → collision.
        var detector = MakeDetector(PointShape);

        var a = MakeCollidable(0, 0, 4, 0, "point");
        var b = MakeCollidable(6, 0, -4, 0, "point");

        Assert.True(detector.Collides(a.Object, b.Object));
    }
}
