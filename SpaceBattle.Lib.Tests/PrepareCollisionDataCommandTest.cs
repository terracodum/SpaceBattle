using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class PrepareCollisionDataCommandTest
{
    [Fact]
    public void Execute_SingleCellShapes_AddsOverlapEntry()
    {
        var shapeA = new ShapeFootprint("ship", [(0, 0)]);
        var shapeB = new ShapeFootprint("torpedo", [(0, 0)]);
        var map = new CollisionMap();

        new PrepareCollisionDataCommand(shapeA, shapeB, map).Execute();

        Assert.True(map.Contains("ship", "torpedo", 0, 0));
    }

    [Fact]
    public void Execute_MultiCellShapeA_AddsEntryForEachCell()
    {
        var shapeA = new ShapeFootprint("ship", [(0, 0), (1, 0), (-1, 0)]);
        var shapeB = new ShapeFootprint("torpedo", [(0, 0)]);
        var map = new CollisionMap();

        new PrepareCollisionDataCommand(shapeA, shapeB, map).Execute();

        Assert.True(map.Contains("ship", "torpedo", 0, 0));
        Assert.True(map.Contains("ship", "torpedo", 1, 0));
        Assert.True(map.Contains("ship", "torpedo", -1, 0));
    }

    [Fact]
    public void Execute_MultiCellBothShapes_AddsAllPairCombinations()
    {
        var shapeA = new ShapeFootprint("A", [(0, 0), (1, 0)]);
        var shapeB = new ShapeFootprint("B", [(0, 0), (0, 1)]);
        var map = new CollisionMap();

        new PrepareCollisionDataCommand(shapeA, shapeB, map).Execute();

        // (0,0)-(0,0) → rel(0,0); (0,0)-(0,1) → rel(0,-1)
        // (1,0)-(0,0) → rel(1,0); (1,0)-(0,1) → rel(1,-1)
        Assert.True(map.Contains("A", "B", 0, 0));
        Assert.True(map.Contains("A", "B", 0, -1));
        Assert.True(map.Contains("A", "B", 1, 0));
        Assert.True(map.Contains("A", "B", 1, -1));
        Assert.Equal(4, map.GetEntries().Count);
    }

    [Fact]
    public void Execute_DisjointPositions_DoesNotAddFalsePositives()
    {
        var shapeA = new ShapeFootprint("ship", [(0, 0)]);
        var shapeB = new ShapeFootprint("torpedo", [(0, 0)]);
        var map = new CollisionMap();

        new PrepareCollisionDataCommand(shapeA, shapeB, map).Execute();

        Assert.False(map.Contains("ship", "torpedo", 1, 0));
        Assert.False(map.Contains("ship", "torpedo", 0, 1));
    }
}
