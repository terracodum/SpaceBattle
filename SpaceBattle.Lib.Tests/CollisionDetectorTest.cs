using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class CollisionDetectorTest
{
    private static Mock<ICollidable> MakeCollidable(int x, int y, int vx, int vy, string shape = "ship")
    {
        var mock = new Mock<ICollidable>();
        mock.Setup(c => c.GetPosition()).Returns(new Vector([x, y]));
        mock.Setup(c => c.GetVelocity()).Returns(new Vector([vx, vy]));
        mock.Setup(c => c.GetShapeId()).Returns(shape);
        return mock;
    }

    [Fact]
    public void Collides_CurrentPositionOverlap_ReturnsTrue()
    {
        var map = new CollisionMap();
        map.Add("ship", "torpedo", 0, 0);
        var detector = new CollisionDetector(map);

        var a = MakeCollidable(5, 5, 0, 0, "ship");
        var b = MakeCollidable(5, 5, 0, 0, "torpedo");

        Assert.True(detector.Collides(a.Object, b.Object));
    }

    [Fact]
    public void Collides_NoOverlapCurrentOrNext_ReturnsFalse()
    {
        var map = new CollisionMap();
        map.Add("ship", "torpedo", 0, 0);
        var detector = new CollisionDetector(map);

        var a = MakeCollidable(0, 0, 1, 0, "ship");
        var b = MakeCollidable(5, 0, 0, 0, "torpedo");

        Assert.False(detector.Collides(a.Object, b.Object));
    }

    [Fact]
    public void Collides_DiscretePassThrough_DetectedAtNextTick()
    {
        // A is at (0,0) moving right with velocity (3,0).
        // B is at (3,0) stationary.
        // They don't overlap now (relX=3 not in map), but WILL at next tick (relX=0).
        var map = new CollisionMap();
        map.Add("ship", "torpedo", 0, 0); // collision when they share the same cell
        var detector = new CollisionDetector(map);

        var a = MakeCollidable(0, 0, 3, 0, "ship");
        var b = MakeCollidable(3, 0, 0, 0, "torpedo");

        Assert.True(detector.Collides(a.Object, b.Object));
    }

    [Fact]
    public void Collides_DifferentShapesThatDoNotCollide_ReturnsFalse()
    {
        var map = new CollisionMap();
        // No entries for these shapes
        var detector = new CollisionDetector(map);

        var a = MakeCollidable(5, 5, 0, 0, "ghost");
        var b = MakeCollidable(5, 5, 0, 0, "torpedo");

        Assert.False(detector.Collides(a.Object, b.Object));
    }

    [Fact]
    public void Collides_UsesMapForBothCurrentAndNextCheck()
    {
        var mockMap = new Mock<ICollisionMap>();
        mockMap.Setup(m => m.Contains(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
               .Returns(false);
        var detector = new CollisionDetector(mockMap.Object);

        var a = MakeCollidable(0, 0, 1, 0);
        var b = MakeCollidable(2, 0, 0, 0);

        detector.Collides(a.Object, b.Object);

        mockMap.Verify(m => m.Contains(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
            Times.Exactly(2));
    }
}
